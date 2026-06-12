using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Materials.Queries.GetLessonMaterials;

public record GetLessonMaterialsQuery(int LessonId) : IRequest<Result<List<LessonMaterialItem>>>;

public record LessonMaterialItem(
  int Id,
  string Title,
  string Type,
  string? Url,
  string? FilePath,
  string? Description,
  DateTime CreatedAt
);
