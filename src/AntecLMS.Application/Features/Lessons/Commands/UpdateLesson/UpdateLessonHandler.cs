using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Lessons.Commands.UpdateLesson;

public class UpdateLessonHandler
  : IRequestHandler<UpdateLessonCommand, Result<LessonUpdatedResponse>>
{
  private readonly ILessonRepository _lessons;
  private readonly IUnitOfWork _uow;

  public UpdateLessonHandler(ILessonRepository lessons, IUnitOfWork uow)
  {
    _lessons = lessons;
    _uow = uow;
  }

  public async Task<Result<LessonUpdatedResponse>> Handle(
    UpdateLessonCommand request,
    CancellationToken ct
  )
  {
    var lesson =
      await _lessons.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Lesson", request.Id);

    var lessonDate = request.LessonDate ?? lesson.LessonDate;
    var topic = request.Topic ?? lesson.Topic;
    var note = request.Note ?? lesson.Note;
    var status = request.Status?.ToLower() switch
    {
      "completed" => LessonStatus.Completed,
      "draft" => LessonStatus.Draft,
      _ => lesson.Status,
    };

    lesson.Update(lessonDate, topic, note, status);
    _lessons.Update(lesson);
    await _uow.SaveChangesAsync(ct);

    return Result<LessonUpdatedResponse>.Success(
      new LessonUpdatedResponse(lesson.Id, lesson.Status.ToString().ToLower())
    );
  }
}
