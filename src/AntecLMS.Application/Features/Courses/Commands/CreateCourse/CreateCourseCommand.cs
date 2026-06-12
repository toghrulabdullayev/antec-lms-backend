using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Commands.CreateCourse;

public record CreateCourseCommand(
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status
) : IRequest<Result<CourseResponse>>;

public record CourseResponse(
  int Id,
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status,
  DateTime CreatedAt
);
