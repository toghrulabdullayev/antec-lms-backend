using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Queries.GetCourseById;

public record GetCourseByIdQuery(int Id) : IRequest<Result<CourseDetailResponse>>;

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
