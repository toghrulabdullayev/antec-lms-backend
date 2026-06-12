using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Queries.GetCourseById;

public class GetCourseByIdHandler
  : IRequestHandler<GetCourseByIdQuery, Result<CourseDetailResponse>>
{
  private readonly ICourseRepository _courses;

  public GetCourseByIdHandler(ICourseRepository courses) => _courses = courses;

  public async Task<Result<CourseDetailResponse>> Handle(
    GetCourseByIdQuery request,
    CancellationToken ct
  )
  {
    var course =
      await _courses.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Course", request.Id);

    return Result<CourseDetailResponse>.Success(
      new CourseDetailResponse(
        course.Id,
        course.Name,
        course.Description,
        course.Price,
        course.ImageUrl,
        course.Status.ToString().ToLower(),
        course.Groups.Count,
        course.CreatedAt
      )
    );
  }
}
