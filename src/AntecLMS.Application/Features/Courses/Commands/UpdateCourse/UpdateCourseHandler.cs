using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Commands.UpdateCourse;

public class UpdateCourseHandler
  : IRequestHandler<UpdateCourseCommand, Result<UpdatedCourseResponse>>
{
  private readonly ICourseRepository _courses;
  private readonly IUnitOfWork _uow;

  public UpdateCourseHandler(ICourseRepository courses, IUnitOfWork uow)
  {
    _courses = courses;
    _uow = uow;
  }

  public async Task<Result<UpdatedCourseResponse>> Handle(
    UpdateCourseCommand request,
    CancellationToken ct
  )
  {
    var course =
      await _courses.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Course", request.Id);

    var status = Enum.Parse<CourseStatus>(request.Status, true);
    course.Update(request.Name, request.Description, request.Price, request.ImageUrl, status);

    _courses.Update(course);
    await _uow.SaveChangesAsync(ct);

    return Result<UpdatedCourseResponse>.Success(
      new UpdatedCourseResponse(
        course.Id,
        course.Name,
        course.Price,
        course.ImageUrl,
        course.Status.ToString().ToLower()
      )
    );
  }
}
