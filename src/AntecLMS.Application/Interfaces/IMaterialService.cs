using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IMaterialService
{
  Task<Result<List<MaterialItem>>> GetByGroupAsync(int groupId, CancellationToken ct);
  Task<Result<List<LessonMaterialItem>>> GetByLessonAsync(int lessonId, CancellationToken ct);
  Task<Result<MaterialResponse>> CreateAsync(CreateMaterialDto dto, CancellationToken ct);
  Task<Result<MaterialResponse>> UpdateAsync(int id, UpdateMaterialDto dto, CancellationToken ct);
  Task<Result> DeleteAsync(int id, CancellationToken ct);
}
