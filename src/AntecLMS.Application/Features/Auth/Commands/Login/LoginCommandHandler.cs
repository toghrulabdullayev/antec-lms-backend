using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
  private readonly IUserRepository _users;
  private readonly IJwtService _jwt;
  private readonly IPasswordHasher _hasher;

  public LoginCommandHandler(IUserRepository users, IJwtService jwt, IPasswordHasher hasher)
  {
    _users = users;
    _jwt = jwt;
    _hasher = hasher;
  }

  public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken ct)
  {
    var user = await _users.GetByEmailAsync(request.Email, ct);

    if (user is null || !_hasher.Verify(request.Password, user.Password))
      return Result<LoginResponse>.Failure("Email və ya şifrə yanlışdır.", 401);

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
}
