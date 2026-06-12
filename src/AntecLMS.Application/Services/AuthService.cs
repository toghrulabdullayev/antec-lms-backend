using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Repositories;

namespace AntecLMS.Application.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _users;
  private readonly IJwtService _jwt;
  private readonly IPasswordHasher _hasher;
  private readonly ICurrentUserService _current;

  public AuthService(
    IUserRepository users,
    IJwtService jwt,
    IPasswordHasher hasher,
    ICurrentUserService current
  )
  {
    _users = users;
    _jwt = jwt;
    _hasher = hasher;
    _current = current;
  }

  public async Task<Result<LoginResponse>> LoginAsync(LoginDto dto, CancellationToken ct)
  {
    var user = await _users.GetByEmailAsync(dto.Email, ct);

    if (user is null || !_hasher.Verify(dto.Password, user.Password))
      return Result<LoginResponse>.Failure("Email veya sifre yanlisdir.", 401);

    var token = _jwt.GenerateToken(user);

    var response = new LoginResponse(
      token,
      new UserDto(
        user.Id,
        user.Name,
        user.Surname,
        user.Email,
        user.Role.ToString().ToLower(),
        user.Status.ToString().ToLower()
      )
    );

    return Result<LoginResponse>.Success(response);
  }

  public async Task<Result<MeResponse>> GetMeAsync(CancellationToken ct)
  {
    var user =
      await _users.GetByIdAsync(_current.UserId, ct)
      ?? throw new NotFoundException("User", _current.UserId);

    return Result<MeResponse>.Success(
      new MeResponse(
        user.Id,
        user.Name,
        user.Surname,
        user.Email,
        user.Role.ToString().ToLower(),
        user.Phone,
        user.Status.ToString().ToLower()
      )
    );
  }

  public Task<Result> LogoutAsync(string token, CancellationToken ct)
  {
    _jwt.InvalidateToken(token);
    return Task.FromResult(Result.Success());
  }
}
