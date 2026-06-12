using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Commands.UpdateGrade;

public class UpdateGradeHandler : IRequestHandler<UpdateGradeCommand, Result<GradeUpdatedResponse>>
{
  private readonly IGradeRepository _grades;
  private readonly IUnitOfWork _uow;

  public UpdateGradeHandler(IGradeRepository grades, IUnitOfWork uow)
  {
    _grades = grades;
    _uow = uow;
  }

  public async Task<Result<GradeUpdatedResponse>> Handle(
    UpdateGradeCommand request,
    CancellationToken ct
  )
  {
    var grade =
      await _grades.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Grade", request.Id);

    grade.Update(request.Score, request.MaxScore, request.TeacherNote);
    _grades.Update(grade);
    await _uow.SaveChangesAsync(ct);

    return Result<GradeUpdatedResponse>.Success(
      new GradeUpdatedResponse(grade.Id, grade.Score, grade.MaxScore)
    );
  }
}
