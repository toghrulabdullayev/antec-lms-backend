using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;

namespace AntecLMS.Application.Services;

public class GradeService : IGradeService
{
  private readonly IGradeRepository _grades;
  private readonly ILessonRepository _lessons;
  private readonly IStudentRepository _students;
  private readonly IUnitOfWork _uow;

  public GradeService(
    IGradeRepository grades,
    ILessonRepository lessons,
    IStudentRepository students,
    IUnitOfWork uow
  )
  {
    _grades = grades;
    _lessons = lessons;
    _students = students;
    _uow = uow;
  }

  public async Task<Result<List<MyGradeItem>>> GetByLessonAsync(int lessonId, CancellationToken ct)
  {
    _ =
      await _lessons.GetByIdAsync(lessonId, ct) ?? throw new NotFoundException("Lesson", lessonId);

    var items = await _grades.GetByLessonAsync(lessonId, ct);

    var data = items
      .Select(g => new MyGradeItem(
        g.Id,
        g.Lesson?.Topic ?? "",
        g.CreatedAt,
        g.Score,
        g.MaxScore,
        g.TeacherNote
      ))
      .ToList();

    return Result<List<MyGradeItem>>.Success(data);
  }

  public async Task<Result<List<StudentGradeItem>>> GetByStudentAsync(
    int studentId,
    CancellationToken ct
  )
  {
    _ =
      await _students.GetByIdAsync(studentId, ct)
      ?? throw new NotFoundException("Student", studentId);

    var items = await _grades.GetByStudentAsync(studentId, ct);

    var data = items
      .Select(g => new StudentGradeItem(
        g.Id,
        g.LessonId,
        g.Lesson?.Topic,
        g.Lesson!.LessonDate,
        g.Score,
        g.MaxScore,
        g.TeacherNote,
        g.CreatedAt
      ))
      .ToList();

    return Result<List<StudentGradeItem>>.Success(data);
  }

  public async Task<Result<GradeResponse>> CreateAsync(
    int lessonId,
    CreateGradeDto dto,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(lessonId, ct) ?? throw new NotFoundException("Lesson", lessonId);
    _ =
      await _students.GetByIdAsync(dto.StudentId, ct)
      ?? throw new NotFoundException("Student", dto.StudentId);

    var grade = Grade.Create(lessonId, dto.StudentId, dto.Score, dto.MaxScore, dto.TeacherNote);

    await _grades.AddAsync(grade, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<GradeResponse>.Success(
      new GradeResponse(
        grade.Id,
        grade.LessonId,
        grade.StudentId,
        grade.Score,
        grade.MaxScore,
        grade.TeacherNote,
        grade.CreatedAt
      ),
      201
    );
  }

  public async Task<Result<GradeResponse>> UpdateAsync(
    int id,
    UpdateGradeDto dto,
    CancellationToken ct
  )
  {
    var grade = await _grades.GetByIdAsync(id, ct) ?? throw new NotFoundException("Grade", id);

    grade.Update(dto.Score, dto.MaxScore, dto.TeacherNote);
    _grades.Update(grade);
    await _uow.SaveChangesAsync(ct);

    return Result<GradeResponse>.Success(
      new GradeResponse(
        grade.Id,
        grade.LessonId,
        grade.StudentId,
        grade.Score,
        grade.MaxScore,
        grade.TeacherNote,
        grade.CreatedAt
      )
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var grade = await _grades.GetByIdAsync(id, ct) ?? throw new NotFoundException("Grade", id);
    _grades.Remove(grade);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }

  Task<Result<List<MyGradeItem>>> IGradeService.GetByLessonAsync(int lessonId, CancellationToken ct)
  {
    throw new NotImplementedException();
  }
}
