using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Attendances.Commands.DeleteAttendance;

public record DeleteAttendanceCommand(int Id) : IRequest<Result>;
