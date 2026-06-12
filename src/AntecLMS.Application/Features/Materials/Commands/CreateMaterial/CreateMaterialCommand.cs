using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Materials.Commands.CreateMaterial;

public record CreateMaterialCommand(
  int LessonId,
  int GroupId,
  int TeacherId,
  string Title,
  string Type,
  string? Url,
  string? FilePath,
  string? Description
) : IRequest<Result<MaterialResponse>>;

public record MaterialResponse(
  int Id,
  int LessonId,
  int GroupId,
  int TeacherId,
  string Title,
  string Type,
  string? Url,
  string? FilePath,
  string? Description,
  DateTime CreatedAt
);
