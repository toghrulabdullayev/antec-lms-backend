using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Lessons.Queries.GetGroupLessons;

public record GetGroupLessonsQuery(int GroupId) : IRequest<Result<List<GroupLessonItem>>>;

public record GroupLessonItem(
  int Id,
  DateTime LessonDate,
  string Topic,
  string Status,
  int AttendanceCount,
  int GradeCount
);
