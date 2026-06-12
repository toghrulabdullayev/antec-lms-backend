using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateCourseDto(
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status
);

public record UpdateCourseDto(
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status
);

public record CourseResponse(
  int Id,
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status,
  DateTime CreatedAt
);

public record CourseListItem(
  int Id,
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status,
  DateTime CreatedAt
);

public record CourseDetailResponse(
  int Id,
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status,
  int GroupsCount,
  DateTime CreatedAt
);
