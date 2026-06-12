using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface ILessonService
{
  Task<Result<List<GroupLessonItem>>> GetByGroupAsync(int groupId, CancellationToken ct);
  Task<Result<LessonDetail>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<LessonResponse>> CreateAsync(CreateLessonDto dto, CancellationToken ct);
  Task<Result<LessonResponse>> UpdateAsync(int id, UpdateLessonDto dto, CancellationToken ct);
  Task<Result> DeleteAsync(int id, CancellationToken ct);
}
