using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Commands.DeleteAttendance;

public class DeleteAttendanceHandler : IRequestHandler<DeleteAttendanceCommand, Result>
{
  private readonly IAttendanceRepository _attendances;
  private readonly IUnitOfWork _uow;

  public DeleteAttendanceHandler(IAttendanceRepository attendances, IUnitOfWork uow)
  {
    _attendances = attendances;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteAttendanceCommand request, CancellationToken ct)
  {
    var attendance =
      await _attendances.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Attendance", request.Id);

    _attendances.Remove(attendance);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
