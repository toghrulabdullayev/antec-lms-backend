using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateLessonDto(
  int GroupId,
  int TeacherId,
  DateTime LessonDate,
  string Topic,
  string? Note,
  string Status,
  string? Type
);

public record UpdateLessonDto(DateTime? LessonDate, string? Topic, string? Note, string? Status, string? Type);

public record LessonDetail(
  int Id,
  int GroupId,
  string? GroupName,
  int TeacherId,
  string? TeacherName,
  DateTime LessonDate,
  string Topic,
  string? Note,
  string Status,
  string Type,
  DateTime CreatedAt,
  List<LessonAttendanceItem> Attendances,
  List<LessonGradeItem> Grades
);

public record LessonAttendanceItem(
  int Id,
  int StudentId,
  string? StudentName,
  string Status,
  int? MinutesLate,
  string? Reason
);

public record LessonGradeItem(int Id, int StudentId, string? StudentName, int Score, int MaxScore);

public record LessonResponse(
  int Id,
  int GroupId,
  int TeacherId,
  DateTime LessonDate,
  string Topic,
  string Status,
  string Type,
  DateTime CreatedAt
);

public record GroupLessonItem(
  int Id,
  DateTime LessonDate,
  string Topic,
  string Status,
  string Type,
  int AttendanceCount,
  int GradeCount
);
