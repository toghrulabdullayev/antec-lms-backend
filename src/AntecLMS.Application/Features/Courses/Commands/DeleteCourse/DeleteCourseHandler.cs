using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Courses.Commands.DeleteCourse;

public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Result>
{
  private readonly ICourseRepository _courses;
  private readonly IUnitOfWork _uow;

  public DeleteCourseHandler(ICourseRepository courses, IUnitOfWork uow)
  {
    _courses = courses;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteCourseCommand request, CancellationToken ct)
  {
    var course =
      await _courses.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Course", request.Id);

    if (await _courses.HasActiveGroupsAsync(request.Id, ct))
      return Result.Failure("Bu kursla bağlı aktiv qruplar var, silinə bilməz.", 400);

    _courses.Remove(course);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}
