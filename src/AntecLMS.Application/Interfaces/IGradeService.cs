using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IGradeService
{
  Task<Result<List<MyGradeItem>>> GetByLessonAsync(int lessonId, CancellationToken ct);
  Task<Result<List<StudentGradeItem>>> GetByStudentAsync(int studentId, CancellationToken ct);
  Task<Result<GradeResponse>> CreateAsync(int lessonId, CreateGradeDto dto, CancellationToken ct);
  Task<Result<GradeResponse>> UpdateAsync(int id, UpdateGradeDto dto, CancellationToken ct);
  Task<Result> DeleteAsync(int id, CancellationToken ct);
}
