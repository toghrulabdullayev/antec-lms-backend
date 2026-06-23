using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateAttendanceDto(
  int StudentId,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote
);

public record UpdateAttendanceDto(
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote
);

public record AttendanceResponse(
  int Id,
  int LessonId,
  int StudentId,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote,
  DateTime CreatedAt
);

public record AttendanceItem(
  int Id,
  int StudentId,
  string? StudentName,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote,
  DateTime CreatedAt
);

public record AttendanceJournalResponse(
  List<AttendanceItem> Items,
  int PresentCount,
  int ExcusedCount,
  int AbsentCount,
  int LateCount,
  double Percentage
);
