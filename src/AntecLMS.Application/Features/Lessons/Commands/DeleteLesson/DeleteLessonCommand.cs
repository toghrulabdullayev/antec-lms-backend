using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Lessons.Commands.DeleteLesson;

public record DeleteLessonCommand(int Id) : IRequest<Result>;
