using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Students.Commands.DeleteStudent;

public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, Result>
{
  private readonly IStudentRepository _students;
  private readonly IUnitOfWork _uow;

  public DeleteStudentHandler(IStudentRepository students, IUnitOfWork uow)
  {
    _students = students;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteStudentCommand request, CancellationToken ct)
  {
    var student =
      await _students.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Student", request.Id);

    _students.Remove(student);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}
