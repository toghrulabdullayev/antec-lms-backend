using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;

namespace AntecLMS.Application.Services;

public class UserService : IUserService
{
  private readonly IUserRepository _users;
  private readonly IGroupRepository _groups;
  private readonly IPasswordHasher _hasher;
  private readonly IUnitOfWork _uow;

  public UserService(
    IUserRepository users,
    IGroupRepository groups,
    IPasswordHasher hasher,
    IUnitOfWork uow
  )
  {
    _users = users;
    _groups = groups;
    _hasher = hasher;
    _uow = uow;
  }

  public async Task<Result<PagedResult<UserListItem>>> GetAllAsync(
    string? role,
    string? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct
  )
  {
    UserRole? roleEnum = role is null ? null : Enum.Parse<UserRole>(role, true);
    UserStatus? statusEnum = status is null ? null : Enum.Parse<UserStatus>(status, true);

    var (items, total) = await _users.GetPagedAsync(
      roleEnum,
      statusEnum,
      search,
      page,
      perPage,
      ct
    );

    var data = items
      .Select(u => new UserListItem(
        u.Id,
        u.Name,
        u.Surname,
        u.Email,
        u.Role.ToString().ToLower(),
        u.Phone,
        u.Status.ToString().ToLower(),
        u.CreatedAt
      ))
      .ToList();

    return Result<PagedResult<UserListItem>>.Success(
      new PagedResult<UserListItem>
      {
        Data = data,
        Total = total,
        Page = page,
        PerPage = perPage,
      }
    );
  }

  public async Task<Result<UserDetailResponse>> GetByIdAsync(int id, CancellationToken ct)
  {
    var user = await _users.GetByIdAsync(id, ct) ?? throw new NotFoundException("User", id);

    return Result<UserDetailResponse>.Success(
      new UserDetailResponse(
        user.Id,
        user.Name,
        user.Surname,
        user.Email,
        user.Role.ToString().ToLower(),
        user.Phone,
        user.Status.ToString().ToLower(),
        user.CreatedAt,
        user.UpdatedAt
      )
    );
  }

  public async Task<Result<UserResponse>> CreateAsync(CreateUserDto dto, CancellationToken ct)
  {
    if (await _users.EmailExistsAsync(dto.Email, ct))
      return Result<UserResponse>.Failure("Bu email artıq istifadə olunur.", 400);

    var user = User.Create(
      dto.Name,
      dto.Surname,
      dto.Email,
      _hasher.Hash(dto.Password),
      Enum.Parse<UserRole>(dto.Role, true),
      dto.Phone,
      Enum.Parse<UserStatus>(dto.Status, true)
    );

    await _users.AddAsync(user, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<UserResponse>.Success(
      new UserResponse(
        user.Id,
        user.Name,
        user.Surname,
        user.Email,
        user.Phone,
        user.Role.ToString().ToLower(),
        user.Status.ToString().ToLower(),
        user.CreatedAt
      ),
      201
    );
  }

  public async Task<Result<UserResponse>> UpdateAsync(
    int id,
    UpdateUserDto dto,
    CancellationToken ct
  )
  {
    var user = await _users.GetByIdAsync(id, ct) ?? throw new NotFoundException("User", id);

    user.Update(dto.Name, dto.Surname, dto.Phone, Enum.Parse<UserStatus>(dto.Status, true));
    _users.Update(user);
    await _uow.SaveChangesAsync(ct);

    return Result<UserResponse>.Success(
      new UserResponse(
        user.Id,
        user.Name,
        user.Surname,
        user.Email,
        user.Phone,
        user.Role.ToString().ToLower(),
        user.Status.ToString().ToLower(),
        user.CreatedAt
      )
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var user = await _users.GetByIdAsync(id, ct) ?? throw new NotFoundException("User", id);

    if (user.TeacherProfile is not null)
    {
      var hasGroups = await _groups.HasActiveGroupsForTeacherAsync(user.TeacherProfile.Id, ct);
      if (hasGroups)
        return Result.Failure("Bu istifadəçinin aktiv qrupları var, silinə bilməz.", 400);
    }

    _users.Remove(user);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
