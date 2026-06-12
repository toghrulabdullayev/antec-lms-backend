using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Commands.DeleteGrade;

public class DeleteGradeHandler : IRequestHandler<DeleteGradeCommand, Result>
{
  private readonly IGradeRepository _grades;
  private readonly IUnitOfWork _uow;

  public DeleteGradeHandler(IGradeRepository grades, IUnitOfWork uow)
  {
    _grades = grades;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteGradeCommand request, CancellationToken ct)
  {
    var grade =
      await _grades.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Grade", request.Id);

    _grades.Remove(grade);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
