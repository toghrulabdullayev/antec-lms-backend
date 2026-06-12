using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Lessons.Commands.UpdateLesson;

public record UpdateLessonCommand(
  int Id,
  DateTime? LessonDate,
  string? Topic,
  string? Note,
  string? Status
) : IRequest<Result<LessonUpdatedResponse>>;

public record LessonUpdatedResponse(int Id, string Status);
