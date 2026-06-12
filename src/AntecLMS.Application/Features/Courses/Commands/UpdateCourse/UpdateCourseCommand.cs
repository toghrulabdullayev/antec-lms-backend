using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Commands.UpdateCourse;

public record UpdateCourseCommand(
  int Id,
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status
) : IRequest<Result<UpdatedCourseResponse>>;

public record UpdatedCourseResponse(
  int Id,
  string Name,
  decimal Price,
  string? ImageUrl,
  string Status
);
