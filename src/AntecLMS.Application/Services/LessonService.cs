using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Services;

public class LessonService : ILessonService
{
  private readonly ILessonRepository _lessons;
  private readonly IGroupRepository _groups;
  private readonly ITeacherRepository _teachers;
  private readonly IUnitOfWork _uow;

  public LessonService(
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

  public async Task<Result<List<GroupLessonItem>>> GetByGroupAsync(
    int groupId,
    CancellationToken ct
  )
  {
    _ = await _groups.GetByIdAsync(groupId, ct) ?? throw new NotFoundException("Group", groupId);

    var lessons = await _lessons
      .GetAll()
      .Where(l => l.GroupId == groupId)
      .Include(l => l.Attendances)
      .Include(l => l.Grades)
      .OrderByDescending(l => l.LessonDate)
      .ToListAsync(ct);

    var data = lessons
      .Select(l => new GroupLessonItem(
        l.Id,
        l.LessonDate,
        l.Topic,
        l.Status.ToString().ToLower(),
        l.Attendances.Count,
        l.Grades.Count
      ))
      .ToList();

    return Result<List<GroupLessonItem>>.Success(data);
  }

  public async Task<Result<LessonDetail>> GetByIdAsync(int id, CancellationToken ct)
  {
    var lesson =
      await _lessons
        .GetAll()
        .Include(l => l.Group)
        .Include(l => l.Teacher)
          .ThenInclude(t => t.User)
        .Include(l => l.Attendances)
          .ThenInclude(a => a.Student)
            .ThenInclude(s => s.User)
        .Include(l => l.Grades)
          .ThenInclude(g => g.Student)
            .ThenInclude(s => s.User)
        .FirstOrDefaultAsync(l => l.Id == id, ct)
      ?? throw new NotFoundException("Lesson", id);

    var detail = new LessonDetail(
      lesson.Id,
      lesson.GroupId,
      lesson.Group?.Name,
      lesson.TeacherId,
      $"{lesson.Teacher?.User?.Name} {lesson.Teacher?.User?.Surname}",
      lesson.LessonDate,
      lesson.Topic,
      lesson.Note,
      lesson.Status.ToString().ToLower(),
      lesson.CreatedAt,
      lesson
        .Attendances.Select(a => new LessonAttendanceItem(
          a.Id,
          a.StudentId,
          $"{a.Student?.User?.Name} {a.Student?.User?.Surname}",
          a.Status.ToString().ToLower(),
          a.MinutesLate,
          a.Reason
        ))
        .ToList(),
      lesson
        .Grades.Select(g => new LessonGradeItem(
          g.Id,
          g.StudentId,
          $"{g.Student?.User?.Name} {g.Student?.User?.Surname}",
          g.Score,
          g.MaxScore
        ))
        .ToList()
    );

    return Result<LessonDetail>.Success(detail);
  }

  public async Task<Result<LessonResponse>> CreateAsync(CreateLessonDto dto, CancellationToken ct)
  {
    _ =
      await _groups.GetByIdAsync(dto.GroupId, ct)
      ?? throw new NotFoundException("Group", dto.GroupId);
    _ =
      await _teachers.GetByIdAsync(dto.TeacherId, ct)
      ?? throw new NotFoundException("Teacher", dto.TeacherId);

    var status = dto.Status?.ToLower() switch
    {
      "completed" => LessonStatus.Completed,
      _ => LessonStatus.Draft,
    };

    var lesson = Lesson.Create(
      dto.GroupId,
      dto.TeacherId,
      dto.LessonDate,
      dto.Topic,
      dto.Note,
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

  public async Task<Result<LessonResponse>> UpdateAsync(
    int id,
    UpdateLessonDto dto,
    CancellationToken ct
  )
  {
    var lesson = await _lessons.GetByIdAsync(id, ct) ?? throw new NotFoundException("Lesson", id);

    var lessonDate = dto.LessonDate ?? lesson.LessonDate;
    var topic = dto.Topic ?? lesson.Topic;
    var note = dto.Note ?? lesson.Note;
    var status = dto.Status?.ToLower() switch
    {
      "completed" => LessonStatus.Completed,
      "draft" => LessonStatus.Draft,
      _ => lesson.Status,
    };

    lesson.Update(lessonDate, topic, note, status);
    _lessons.Update(lesson);
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
      )
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var lesson = await _lessons.GetByIdAsync(id, ct) ?? throw new NotFoundException("Lesson", id);
    _lessons.Remove(lesson);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
