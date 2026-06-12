using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateStudentDto(
  string Name,
  string Surname,
  string Email,
  string Password,
  string? Phone,
  DateOnly? BirthDate,
  string? Note,
  string Status
);

public record UpdateStudentDto(string? Phone, string? Note, string Status);

public record StudentResponse(
  int Id,
  int UserId,
  string Name,
  string Surname,
  string Email,
  string Status
);

public record StudentListItem(
  int Id,
  int UserId,
  string Name,
  string Surname,
  string Email,
  string? Phone,
  DateOnly? BirthDate,
  string Status,
  List<GroupRefS> Groups
);

public record GroupRefS(int Id, string Name);

public record StudentDetailResponse(
  int Id,
  int UserId,
  string Name,
  string Surname,
  string Email,
  string? Phone,
  DateOnly? BirthDate,
  string? Note,
  string Status,
  List<GroupRefD> Groups
);

public record GroupRefD(int Id, string Name);

public record StudentAttendanceItem(
  int Id,
  int LessonId,
  string? LessonTopic,
  DateTime LessonDate,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote,
  DateTime CreatedAt
);

public record StudentGradeItem(
  int Id,
  int LessonId,
  string? LessonTopic,
  DateTime LessonDate,
  int Score,
  int MaxScore,
  string? TeacherNote,
  DateTime CreatedAt
);
