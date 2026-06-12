using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Queries.GetCourses;

public class GetCoursesHandler
  : IRequestHandler<GetCoursesQuery, Result<PagedResult<CourseListItem>>>
{
  private readonly ICourseRepository _courses;

  public GetCoursesHandler(ICourseRepository courses) => _courses = courses;

  public async Task<Result<PagedResult<CourseListItem>>> Handle(
    GetCoursesQuery request,
    CancellationToken ct
  )
  {
    CourseStatus? status = request.Status is null
      ? null
      : Enum.Parse<CourseStatus>(request.Status, true);

    var (items, total) = await _courses.GetPagedAsync(
      status,
      request.Search,
      request.Page,
      request.PerPage,
      ct
    );

    var data = items
      .Select(c => new CourseListItem(
        c.Id,
        c.Name,
        c.Description,
        c.Price,
        c.ImageUrl,
        c.Status.ToString().ToLower(),
        c.CreatedAt
      ))
      .ToList();

    return Result<PagedResult<CourseListItem>>.Success(
      new PagedResult<CourseListItem>
      {
        Data = data,
        Total = total,
        Page = request.Page,
        PerPage = request.PerPage,
      }
    );
  }
}
