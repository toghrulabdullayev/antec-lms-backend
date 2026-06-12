using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IGroupService
{
  Task<Result<PagedResult<GroupListItem>>> GetAllAsync(
    int? courseId,
    int? teacherId,
    string? status,
    int page,
    int perPage,
    CancellationToken ct
  );
  Task<Result<GroupDetailResponse>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<GroupResponse>> CreateAsync(CreateGroupDto dto, CancellationToken ct);
  Task<Result<GroupResponse>> UpdateAsync(int id, UpdateGroupDto dto, CancellationToken ct);
  Task<Result> DeleteAsync(int id, CancellationToken ct);
  Task<Result> AddStudentAsync(int groupId, int studentId, CancellationToken ct);
  Task<Result> RemoveStudentAsync(int groupId, int studentId, CancellationToken ct);
}
