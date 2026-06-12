using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Materials.Queries.GetGroupMaterials;

public record GetGroupMaterialsQuery(int GroupId) : IRequest<Result<List<MaterialItem>>>;

public record MaterialItem(
  int Id,
  int LessonId,
  string? LessonTopic,
  string Title,
  string Type,
  string? Url,
  string? FilePath,
  string? Description,
  DateTime CreatedAt
);
