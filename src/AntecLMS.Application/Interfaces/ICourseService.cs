using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface ICourseService
{
  Task<Result<PagedResult<CourseListItem>>> GetAllAsync(
    string? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct
  );
  Task<Result<CourseDetailResponse>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<CourseResponse>> CreateAsync(CreateCourseDto dto, CancellationToken ct);
  Task<Result<CourseResponse>> UpdateAsync(int id, UpdateCourseDto dto, CancellationToken ct);
  Task<Result> DeleteAsync(int id, CancellationToken ct);
}
