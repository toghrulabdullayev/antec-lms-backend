using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateMaterialDto(
  int LessonId,
  int GroupId,
  int TeacherId,
  string Title,
  string Type,
  string? Url,
  string? FilePath,
  string? Description
);

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

public record LessonMaterialItem(
  int Id,
  string Title,
  string Type,
  string? Url,
  string? FilePath,
  string? Description,
  DateTime CreatedAt
);
