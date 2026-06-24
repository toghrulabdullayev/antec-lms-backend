using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface ITeacherService
{
  Task<Result<PagedResult<TeacherListItem>>> GetAllAsync(
    int page,
    int perPage,
    CancellationToken ct
  );
  Task<Result<TeacherDetailResponse>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<TeacherDetailResponse>> GetMyProfileAsync(int userId, CancellationToken ct);
  Task<Result<TeacherResponse>> CreateAsync(CreateTeacherDto dto, CancellationToken ct);
  Task<Result<TeacherResponse>> UpdateAsync(int id, UpdateTeacherDto dto, CancellationToken ct);
  Task<Result> DeleteAsync(int id, CancellationToken ct);
  Task<Result> HardDeleteAsync(int id, CancellationToken ct);
  Task<Result> ChangePasswordAsync(
    int userId,
    string currentPassword,
    string newPassword,
    CancellationToken ct
  );
}
