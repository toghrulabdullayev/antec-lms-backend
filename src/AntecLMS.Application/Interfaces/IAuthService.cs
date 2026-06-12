using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IAuthService
{
  Task<Result<LoginResponse>> LoginAsync(LoginDto dto, CancellationToken ct);
  Task<Result<MeResponse>> GetMeAsync(CancellationToken ct);
  Task<Result> LogoutAsync(string token, CancellationToken ct);
}
