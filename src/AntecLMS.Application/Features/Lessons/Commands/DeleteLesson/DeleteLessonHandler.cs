using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Lessons.Commands.DeleteLesson;

public class DeleteLessonHandler : IRequestHandler<DeleteLessonCommand, Result>
{
  private readonly ILessonRepository _lessons;
  private readonly IUnitOfWork _uow;

  public DeleteLessonHandler(ILessonRepository lessons, IUnitOfWork uow)
  {
    _lessons = lessons;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteLessonCommand request, CancellationToken ct)
  {
    var lesson =
      await _lessons.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Lesson", request.Id);

    _lessons.Remove(lesson);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
