using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Services;

public class ReportService : IReportService
{
  private readonly IGroupRepository _groups;
  private readonly IAttendanceRepository _attendances;
  private readonly IGradeRepository _grades;
  private readonly ILessonRepository _lessons;

  public ReportService(
    IGroupRepository groups,
    IAttendanceRepository attendances,
    IGradeRepository grades,
    ILessonRepository lessons
  )
  {
    _groups = groups;
    _attendances = attendances;
    _grades = grades;
    _lessons = lessons;
  }

  public async Task<Result<AttendanceReportResult>> GetAttendanceReportAsync(
    int groupId,
    DateTime? from,
    DateTime? to,
    CancellationToken ct
  )
  {
    var group =
      await _groups.GetByIdAsync(groupId, ct) ?? throw new NotFoundException("Group", groupId);

    var query = _attendances
      .GetAll()
      .Include(a => a.Student)
        .ThenInclude(s => s.User)
      .Include(a => a.Lesson)
      .Where(a => a.Lesson.GroupId == groupId);

    if (from.HasValue)
      query = query.Where(a => a.Lesson.LessonDate >= from.Value);
    if (to.HasValue)
      query = query.Where(a => a.Lesson.LessonDate <= to.Value);

    var records = await query.ToListAsync(ct);

    var byStudent = records
      .GroupBy(a => new
      {
        a.StudentId,
        Name = $"{a.Student?.User?.Name} {a.Student?.User?.Surname}",
      })
      .Select(g => new AttendanceReportDetail(
        g.Key.StudentId,
        g.Key.Name,
        g.Count(a => a.Status == AttendanceStatus.Present),
        g.Count(a => a.Status == AttendanceStatus.Absent),
        g.Count(a => a.Status == AttendanceStatus.Late),
        g.Count(a => a.Status == AttendanceStatus.Excused),
        Math.Round((double)g.Count(a => a.Status != AttendanceStatus.Absent) / g.Count() * 100, 1)
      ))
      .ToList();

    var total = records.Count;
    var present = records.Count(a => a.Status == AttendanceStatus.Present);
    var absent = records.Count(a => a.Status == AttendanceStatus.Absent);
    var late = records.Count(a => a.Status == AttendanceStatus.Late);
    var excused = records.Count(a => a.Status == AttendanceStatus.Excused);

    return Result<AttendanceReportResult>.Success(
      new AttendanceReportResult(
        records.Select(a => a.LessonId).Distinct().Count(),
        total,
        present,
        absent,
        late,
        excused,
        total > 0 ? Math.Round((double)(total - absent) / total * 100, 1) : 0,
        byStudent
      )
    );
  }

  public async Task<Result<GradesReportResult>> GetGradesReportAsync(
    int groupId,
    DateTime? from,
    DateTime? to,
    CancellationToken ct
  )
  {
    var group =
      await _groups.GetByIdAsync(groupId, ct) ?? throw new NotFoundException("Group", groupId);

    var query = _grades
      .GetAll()
      .Include(g => g.Student)
        .ThenInclude(s => s.User)
      .Include(g => g.Lesson)
      .Where(g => g.Lesson.GroupId == groupId);

    if (from.HasValue)
      query = query.Where(g => g.Lesson.LessonDate >= from.Value);
    if (to.HasValue)
      query = query.Where(g => g.Lesson.LessonDate <= to.Value);

    var records = await query.ToListAsync(ct);

    var byStudent = records
      .GroupBy(g => new
      {
        g.StudentId,
        Name = $"{g.Student?.User?.Name} {g.Student?.User?.Surname}",
      })
      .Select(g => new GradesReportDetail(
        g.Key.StudentId,
        g.Key.Name,
        g.Sum(x => x.Score),
        g.Sum(x => x.MaxScore),
        g.Sum(x => x.MaxScore) > 0
          ? Math.Round((double)g.Sum(x => x.Score) / g.Sum(x => x.MaxScore) * 100, 1)
          : 0,
        g.Count()
      ))
      .ToList();

    var avgScore = records.Any() ? records.Average(x => (double)x.Score) : 0;
    var avgMaxScore = records.Any() ? records.Average(x => (double)x.MaxScore) : 0;
    var totalScore = records.Sum(x => x.Score);
    var totalMaxScore = records.Sum(x => x.MaxScore);
    var overallPct =
      totalMaxScore > 0 ? Math.Round((double)totalScore / totalMaxScore * 100, 1) : 0;

    return Result<GradesReportResult>.Success(
      new GradesReportResult(
        records.Count,
        Math.Round(avgScore, 1),
        Math.Round(avgMaxScore, 1),
        overallPct,
        byStudent
      )
    );
  }

  public async Task<Result<StudentProgressResult>> GetStudentProgressAsync(
    int studentId,
    CancellationToken ct
  )
  {
    var attendances = await _attendances
      .GetAll()
      .Include(a => a.Lesson)
      .Where(a => a.StudentId == studentId)
      .OrderByDescending(a => a.Lesson.LessonDate)
      .ToListAsync(ct);

    var grades = await _grades
      .GetAll()
      .Include(g => g.Lesson)
      .Where(g => g.StudentId == studentId)
      .OrderByDescending(g => g.Lesson.LessonDate)
      .ToListAsync(ct);

    var studentName =
      attendances.FirstOrDefault()?.Student != null
        ? $"{attendances.First().Student?.User?.Name} {attendances.First().Student?.User?.Surname}"
        : null;

    var totalLessons = attendances.Count;
    var attendedLessons = attendances.Count(a => a.Status != AttendanceStatus.Absent);
    var attendanceRate =
      totalLessons > 0 ? Math.Round((double)attendedLessons / totalLessons * 100, 1) : 0;

    var totalScore = grades.Sum(g => g.Score);
    var totalMaxScore = grades.Sum(g => g.MaxScore);
    var gradePct = totalMaxScore > 0 ? Math.Round((double)totalScore / totalMaxScore * 100, 1) : 0;

    var recentActivity = new List<ProgressActivityItem>();
    foreach (var a in attendances.Take(10))
    {
      recentActivity.Add(
        new ProgressActivityItem(
          "attendance",
          $"{a.Lesson?.Topic} ({a.Lesson?.LessonDate:d})",
          a.Lesson?.LessonDate ?? DateTime.MinValue,
          a.Status.ToString().ToLower()
        )
      );
    }
    foreach (var g in grades.Take(10))
    {
      recentActivity.Add(
        new ProgressActivityItem(
          "grade",
          $"{g.Lesson?.Topic} ({g.Lesson?.LessonDate:d})",
          g.Lesson?.LessonDate ?? DateTime.MinValue,
          $"{g.Score}/{g.MaxScore}"
        )
      );
    }
    recentActivity = recentActivity.OrderByDescending(x => x.Date).Take(10).ToList();

    return Result<StudentProgressResult>.Success(
      new StudentProgressResult(
        studentId,
        studentName,
        totalLessons,
        attendedLessons,
        attendanceRate,
        grades.Count,
        totalScore,
        totalMaxScore,
        gradePct,
        recentActivity
      )
    );
  }

  public async Task<Result<GroupAttendanceStatsResult>> GetGroupAttendanceStatsAsync(
    int groupId,
    CancellationToken ct
  )
  {
    var group =
      await _groups.GetByIdAsync(groupId, ct) ?? throw new NotFoundException("Group", groupId);

    var lessons = await _lessons
      .GetAll()
      .Where(l => l.GroupId == groupId)
      .OrderBy(l => l.LessonDate)
      .ToListAsync(ct);

    var attendances = await _attendances
      .GetAll()
      .Include(a => a.Lesson)
      .Where(a => a.Lesson.GroupId == groupId)
      .ToListAsync(ct);

    var byStudent = attendances.GroupBy(a => a.StudentId).ToList();

    var totalStudents = byStudent.Count;
    var totalLessons = lessons.Count;

    var lessonStats = lessons
      .Select(l =>
      {
        var lesAtt = attendances.Where(a => a.LessonId == l.Id).ToList();
        var present = lesAtt.Count(a => a.Status == AttendanceStatus.Present);
        var absent = lesAtt.Count(a => a.Status == AttendanceStatus.Absent);
        var late = lesAtt.Count(a => a.Status == AttendanceStatus.Late);
        var excused = lesAtt.Count(a => a.Status == AttendanceStatus.Excused);
        var totalLes = lesAtt.Count;
        return new LessonAttendanceStat(
          l.Id,
          l.LessonDate,
          l.Topic,
          present,
          absent,
          late,
          excused,
          totalLes > 0 ? Math.Round((double)(totalLes - absent) / totalLes * 100, 1) : 0
        );
      })
      .ToList();

    var avgRate = totalLessons > 0 ? Math.Round(lessonStats.Average(x => x.AttendanceRate), 1) : 0;

    var atRiskCount = byStudent.Count(g =>
    {
      var total = g.Count();
      var absent = g.Count(a => a.Status == AttendanceStatus.Absent);
      return total > 0 && (double)absent / total > 0.3;
    });

    return Result<GroupAttendanceStatsResult>.Success(
      new GroupAttendanceStatsResult(totalStudents, totalLessons, avgRate, atRiskCount, lessonStats)
    );
  }

  public async Task<Result<List<AtRiskStudentItem>>> GetAtRiskStudentsAsync(
    int groupId,
    double? threshold,
    CancellationToken ct
  )
  {
    var group =
      await _groups.GetByIdAsync(groupId, ct) ?? throw new NotFoundException("Group", groupId);

    var t = threshold ?? 0.3;

    var attendances = await _attendances
      .GetAll()
      .Include(a => a.Student)
        .ThenInclude(s => s.User)
      .Include(a => a.Lesson)
      .Where(a => a.Lesson.GroupId == groupId)
      .ToListAsync(ct);

    var grades = await _grades
      .GetAll()
      .Include(g => g.Lesson)
      .Where(g => g.Lesson.GroupId == groupId)
      .ToListAsync(ct);

    var atRisk = attendances
      .GroupBy(a => new
      {
        a.StudentId,
        Name = $"{a.Student?.User?.Name} {a.Student?.User?.Surname}",
      })
      .Where(g =>
      {
        var total = g.Count();
        var absent = g.Count(a => a.Status == AttendanceStatus.Absent);
        return total > 0 && (double)absent / total > t;
      })
      .Select(g =>
      {
        var studentGrades = grades.Where(x => x.StudentId == g.Key.StudentId).ToList();
        var avgGrade = studentGrades.Any()
          ? Math.Round(
            (double)studentGrades.Sum(x => x.Score) / studentGrades.Sum(x => x.MaxScore) * 100,
            1
          )
          : (double?)null;

        return new AtRiskStudentItem(
          g.Key.StudentId,
          g.Key.Name,
          g.Count(),
          g.Count(a => a.Status == AttendanceStatus.Absent),
          Math.Round(
            (double)g.Count(a => a.Status == AttendanceStatus.Absent) / g.Count() * 100,
            1
          ),
          avgGrade
        );
      })
      .OrderByDescending(x => x.AbsentRate)
      .ToList();

    return Result<List<AtRiskStudentItem>>.Success(atRisk);
  }
}
