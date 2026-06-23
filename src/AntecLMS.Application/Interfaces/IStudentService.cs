using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IStudentService
{
  Task<Result<PagedResult<StudentListItem>>> GetAllAsync(
    int? groupId,
    string? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct
  );
  Task<Result<StudentDetailResponse>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<StudentResponse>> CreateAsync(CreateStudentDto dto, CancellationToken ct);
  Task<Result<StudentResponse>> UpdateAsync(int id, UpdateStudentDto dto, CancellationToken ct);
  Task<Result> DeleteAsync(int id, CancellationToken ct);
  Task<Result<List<StudentAttendanceItem>>> GetAttendancesAsync(int studentId, CancellationToken ct);
  Task<Result<List<StudentGradeItem>>> GetGradesAsync(int studentId, CancellationToken ct);
}