using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Queries.GetCourses;

public record GetCoursesQuery(string? Status, string? Search, int Page = 1, int PerPage = 20)
  : IRequest<Result<PagedResult<CourseListItem>>>;

public record CourseListItem(
  int Id,
  string Name,
  string? Description,
  decimal Price,
  string? ImageUrl,
  string Status,
  DateTime CreatedAt
);
