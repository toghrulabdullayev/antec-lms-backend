using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateGroupDto(
  string Name,
  int CourseId,
  int TeacherId,
  DateOnly StartDate,
  DateOnly? EndDate,
  string Status
);

public record UpdateGroupDto(string Name, int TeacherId, string Status);

public record GroupResponse(
  int Id,
  string Name,
  int CourseId,
  int TeacherId,
  DateOnly StartDate,
  string Status
);

public record GroupListItem(
  int Id,
  string Name,
  CourseRef Course,
  TeacherRef Teacher,
  int StudentsCount,
  DateOnly StartDate,
  DateOnly? EndDate,
  string Status
);

public record CourseRef(int Id, string Name);

public record TeacherRef(int Id, string Name, string Surname);

public record GroupDetailResponse(
  int Id,
  string Name,
  CourseInfo Course,
  TeacherInfo Teacher,
  DateOnly StartDate,
  DateOnly? EndDate,
  string Status,
  List<StudentInGroup> Students,
  int StudentsCount
);

public record CourseInfo(int Id, string Name);

public record TeacherInfo(int Id, string Name, string Surname);

public record StudentInGroup(int Id, string Name, string Surname, string Status);

public record AddStudentToGroupDto(int StudentId);

public record MyGroupDetail(
    int Id,
    string Name,
    int LessonCount,
    double AverageGrade,
    string Status
);