namespace AntecLMS.Domain.Enums;

public static class AttendanceStatusExtensions
{
  public static string ToApiString(this AttendanceStatus status) =>
    status switch
    {
      AttendanceStatus.Present => "present",
      AttendanceStatus.Late => "late",
      AttendanceStatus.AbsentExcused => "absent_excused",
      AttendanceStatus.AbsentUnexcused => "absent_unexcused",
      _ => status.ToString().ToLower(),
    };
}
