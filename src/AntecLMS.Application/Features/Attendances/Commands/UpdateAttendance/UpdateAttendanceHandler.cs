using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Commands.UpdateAttendance;

public class UpdateAttendanceHandler
  : IRequestHandler<UpdateAttendanceCommand, Result<AttendanceUpdatedResponse>>
{
  private readonly IAttendanceRepository _attendances;
  private readonly IUnitOfWork _uow;

  public UpdateAttendanceHandler(IAttendanceRepository attendances, IUnitOfWork uow)
  {
    _attendances = attendances;
    _uow = uow;
  }

  public async Task<Result<AttendanceUpdatedResponse>> Handle(
    UpdateAttendanceCommand request,
    CancellationToken ct
  )
  {
    var attendance =
      await _attendances.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Attendance", request.Id);

    var status = Enum.Parse<AttendanceStatus>(request.Status, true);
    attendance.Update(status, request.MinutesLate, request.Reason, request.TeacherNote);

    _attendances.Update(attendance);
    await _uow.SaveChangesAsync(ct);

    return Result<AttendanceUpdatedResponse>.Success(
      new AttendanceUpdatedResponse(attendance.Id, attendance.Status.ToString().ToLower())
    );
  }
}
