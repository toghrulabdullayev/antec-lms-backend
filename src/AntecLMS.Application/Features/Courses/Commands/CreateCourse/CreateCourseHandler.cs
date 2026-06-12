using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<CourseResponse>>
{
  private readonly ICourseRepository _courses;
  private readonly IUnitOfWork _uow;

  public CreateCourseHandler(ICourseRepository courses, IUnitOfWork uow)
  {
    _courses = courses;
    _uow = uow;
  }

  public async Task<Result<CourseResponse>> Handle(
    CreateCourseCommand request,
    CancellationToken ct
  )
  {
    var status = Enum.Parse<CourseStatus>(request.Status, true);
    var course = Course.Create(
      request.Name,
      request.Description,
      request.Price,
      request.ImageUrl,
      status
    );

    await _courses.AddAsync(course, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<CourseResponse>.Success(
      new CourseResponse(
        course.Id,
        course.Name,
        course.Description,
        course.Price,
        course.ImageUrl,
        course.Status.ToString().ToLower(),
        course.CreatedAt
      ),
      201
    );
  }
}
