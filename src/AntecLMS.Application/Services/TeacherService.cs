using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Services;

public class TeacherService : ITeacherService
{
  private readonly IUserRepository _users;
  private readonly ITeacherRepository _teachers;
  private readonly IGroupRepository _groups;
  private readonly IUnitOfWork _uow;
  private readonly IPasswordHasher _hasher;

  public TeacherService(
    IUserRepository users,
    ITeacherRepository teachers,
    IGroupRepository groups,
    IUnitOfWork uow,
    IPasswordHasher hasher
  )
  {
    _users = users;
    _teachers = teachers;
    _groups = groups;
    _uow = uow;
    _hasher = hasher;
  }

  public async Task<Result<PagedResult<TeacherListItem>>> GetAllAsync(
    int page,
    int perPage,
    CancellationToken ct
  )
  {
    var (items, total) = await _teachers.GetPagedAsync(page, perPage, ct);

    var data = items
      .Select(t => new TeacherListItem(
        t.Id,
        t.UserId,
        t.User.Name,
        t.User.Surname,
        t.User.Email,
        t.User.Phone,
        t.Specialization,
        t.Bio,
        t.Status.ToString().ToLower()
      ))
      .ToList();

    return Result<PagedResult<TeacherListItem>>.Success(
      new PagedResult<TeacherListItem>
      {
        Data = data,
        Total = total,
        Page = page,
        PerPage = perPage,
      }
    );
  }

  public async Task<Result<TeacherDetailResponse>> GetByIdAsync(int id, CancellationToken ct)
  {
    var teacher =
      await _teachers
        .GetAll()
        .Include(t => t.User)
        .Include(t => t.Groups)
        .FirstOrDefaultAsync(t => t.Id == id, ct)
      ?? throw new NotFoundException("Teacher", id);

    var groups = teacher.Groups.Select(g => new GroupRefT(g.Id, g.Name)).ToList();

    return Result<TeacherDetailResponse>.Success(
      new TeacherDetailResponse(
        teacher.Id,
        teacher.UserId,
        teacher.User.Name,
        teacher.User.Surname,
        teacher.User.Email,
        teacher.User.Phone,
        teacher.Specialization,
        teacher.Bio,
        teacher.Status.ToString().ToLower(),
        groups
      )
    );
  }

  public async Task<Result<TeacherDetailResponse>> GetMyProfileAsync(
    int userId,
    CancellationToken ct
  )
  {
    var teacher = await _teachers
      .GetAll()
      .Include(t => t.User)
      .Include(t => t.Groups)
      .FirstOrDefaultAsync(t => t.UserId == userId, ct);

    if (teacher is null)
      return Result<TeacherDetailResponse>.Failure("Müəllim tapılmadı", 404);

    var groups = teacher.Groups.Select(g => new GroupRefT(g.Id, g.Name)).ToList();

    return Result<TeacherDetailResponse>.Success(
      new TeacherDetailResponse(
        teacher.Id,
        teacher.UserId,
        teacher.User.Name,
        teacher.User.Surname,
        teacher.User.Email,
        teacher.User.Phone,
        teacher.Specialization,
        teacher.Bio,
        teacher.Status.ToString().ToLower(),
        groups
      )
    );
  }

  public async Task<Result<TeacherResponse>> CreateAsync(CreateTeacherDto dto, CancellationToken ct)
  {
    if (await _users.EmailExistsAsync(dto.Email, ct))
      return Result<TeacherResponse>.Failure("Bu email artıq mövcuddur.", 422);

    var status = Enum.Parse<UserStatus>(dto.Status, true);

    var user = User.Create(
      dto.Name,
      dto.Surname,
      dto.Email,
      _hasher.Hash(dto.Password),
      UserRole.Teacher,
      dto.Phone,
      status
    );

    await _users.AddAsync(user, ct);
    await _uow.SaveChangesAsync(ct);

    var teacher = Teacher.Create(user.Id, dto.Specialization, dto.Bio, status);
    await _teachers.AddAsync(teacher, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<TeacherResponse>.Success(
      new TeacherResponse(
        teacher.Id,
        user.Id,
        user.Name,
        user.Surname,
        user.Email,
        user.Phone,
        teacher.Specialization,
        teacher.Bio,
        teacher.Status.ToString().ToLower()
      ),
      201
    );
  }

  public async Task<Result<TeacherResponse>> UpdateAsync(
    int id,
    UpdateTeacherDto dto,
    CancellationToken ct
  )
  {
    var teacher =
      await _teachers.GetAll().Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id, ct)
      ?? throw new NotFoundException("Teacher", id);

    if (teacher.User is not null)
    {
      var name = dto.Name ?? teacher.User.Name;
      var surname = dto.Surname ?? teacher.User.Surname;
      var email = dto.Email ?? teacher.User.Email;
      var phone = dto.Phone ?? teacher.User.Phone;
      var userStatus = Enum.Parse<UserStatus>(dto.Status, true);

      if (
        dto.Email is not null
        && dto.Email != teacher.User.Email
        && await _users.EmailExistsAsync(dto.Email, ct)
      )
        return Result<TeacherResponse>.Failure("Bu email artıq mövcuddur.", 422);

      teacher.User.Update(name, surname, email, phone, userStatus);

      if (!string.IsNullOrWhiteSpace(dto.Password))
        teacher.User.ChangePassword(_hasher.Hash(dto.Password));
    }

    var bio = dto.Bio ?? teacher.Bio;
    teacher.Update(dto.Specialization, bio, Enum.Parse<Domain.Enums.UserStatus>(dto.Status, true));
    _teachers.Update(teacher);
    await _uow.SaveChangesAsync(ct);

    return Result<TeacherResponse>.Success(
      new TeacherResponse(
        teacher.Id,
        teacher.UserId,
        teacher.User?.Name ?? "",
        teacher.User?.Surname ?? "",
        teacher.User?.Email ?? "",
        teacher.User?.Phone,
        teacher.Specialization,
        teacher.Bio,
        teacher.Status.ToString().ToLower()
      )
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var teacher =
      await _teachers.GetAll().Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id, ct)
      ?? throw new NotFoundException("Teacher", id);

    if (await _groups.HasActiveGroupsForTeacherAsync(id, ct))
      return Result.Failure("Bu müəllimin aktiv qrupları var, silinə bilməz.", 400);

    teacher.Update(teacher.Specialization, teacher.Bio, UserStatus.Inactive);
    if (teacher.User is not null)
      teacher.User.ChangeStatus(UserStatus.Inactive);

    _teachers.Update(teacher);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }

  public async Task<Result> HardDeleteAsync(int id, CancellationToken ct)
  {
    var teacher =
      await _teachers.GetAll().Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id, ct)
      ?? throw new NotFoundException("Teacher", id);

    if (teacher.User is not null)
      _users.Remove(teacher.User);
    _teachers.Remove(teacher);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }

  public async Task<Result> ChangePasswordAsync(
    int userId,
    string currentPassword,
    string newPassword,
    CancellationToken ct
  )
  {
    var user = await _users.GetByIdAsync(userId, ct);
    if (user is null)
      return Result.Failure("İstifadəçi tapılmadı", 404);

    if (!_hasher.Verify(currentPassword, user.Password))
      return Result.Failure("Mövcud şifrə yanlışdır", 400);

    user.ChangePassword(_hasher.Hash(newPassword));
    _users.Update(user);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
