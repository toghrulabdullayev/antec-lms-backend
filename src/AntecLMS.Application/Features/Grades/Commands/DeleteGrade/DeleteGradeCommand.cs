using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Commands.DeleteGrade;

public record DeleteGradeCommand(int Id) : IRequest<Result>;
