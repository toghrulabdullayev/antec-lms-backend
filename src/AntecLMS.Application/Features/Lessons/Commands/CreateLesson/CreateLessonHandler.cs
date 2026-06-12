using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Lessons.Commands.CreateLesson;

public class CreateLessonHandler : IRequestHandler<CreateLessonCommand, Result<LessonResponse>>
{
  private readonly ILessonRepository _lessons;
  private readonly IGroupRepository _groups;
  private readonly ITeacherRepository _teachers;
  private readonly IUnitOfWork _uow;

  public CreateLessonHandler(
    ILessonRepository lessons,
    IGroupRepository groups,
    ITeacherRepository teachers,
    IUnitOfWork uow
  )
  {
    _lessons = lessons;
    _groups = groups;
    _teachers = teachers;
    _uow = uow;
  }

  public async Task<Result<LessonResponse>> Handle(
    CreateLessonCommand request,
    CancellationToken ct
  )
  {
    _ =
      await _groups.GetByIdAsync(request.GroupId, ct)
      ?? throw new NotFoundException("Group", request.GroupId);
    _ =
      await _teachers.GetByIdAsync(request.TeacherId, ct)
      ?? throw new NotFoundException("Teacher", request.TeacherId);

    var status = request.Status?.ToLower() switch
    {
      "completed" => Domain.Enums.LessonStatus.Completed,
      _ => Domain.Enums.LessonStatus.Draft,
    };

    var lesson = Lesson.Create(
      request.GroupId,
      request.TeacherId,
      request.LessonDate,
      request.Topic,
      request.Note,
      status
    );

    await _lessons.AddAsync(lesson, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<LessonResponse>.Success(
      new LessonResponse(
        lesson.Id,
        lesson.GroupId,
        lesson.TeacherId,
        lesson.LessonDate,
        lesson.Topic,
        lesson.Status.ToString().ToLower(),
        lesson.CreatedAt
      ),
      201
    );
  }
}
