using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Grades.Commands.UpdateGrade;

public record UpdateGradeCommand(int Id, int Score, int MaxScore, string? TeacherNote)
  : IRequest<Result<GradeUpdatedResponse>>;

public record GradeUpdatedResponse(int Id, int Score, int MaxScore);
