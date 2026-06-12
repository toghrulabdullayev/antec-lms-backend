using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Commands.CreateGrade;

public class CreateGradeHandler : IRequestHandler<CreateGradeCommand, Result<GradeResponse>>
{
  private readonly IGradeRepository _grades;
  private readonly ILessonRepository _lessons;
  private readonly IStudentRepository _students;
  private readonly IUnitOfWork _uow;

  public CreateGradeHandler(
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

  public async Task<Result<GradeResponse>> Handle(CreateGradeCommand request, CancellationToken ct)
  {
    _ =
      await _lessons.GetByIdAsync(request.LessonId, ct)
      ?? throw new NotFoundException("Lesson", request.LessonId);
    _ =
      await _students.GetByIdAsync(request.StudentId, ct)
      ?? throw new NotFoundException("Student", request.StudentId);

    var grade = Grade.Create(
      request.LessonId,
      request.StudentId,
      request.Score,
      request.MaxScore,
      request.TeacherNote
    );

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
}
