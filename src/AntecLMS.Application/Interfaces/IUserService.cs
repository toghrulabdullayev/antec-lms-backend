using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IUserService
{
  Task<Result<PagedResult<UserListItem>>> GetAllAsync(
    string? role,
    string? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct
  );
  Task<Result<UserDetailResponse>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<UserResponse>> CreateAsync(CreateUserDto dto, CancellationToken ct);
  Task<Result<UserResponse>> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct);
  Task<Result> DeleteAsync(int id, CancellationToken ct);
}
