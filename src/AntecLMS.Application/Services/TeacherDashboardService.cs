using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Services;

public class TeacherDashboardService : ITeacherDashboardService
{
  private readonly ITeacherRepository _teachers;
  private readonly IGroupRepository _groups;
  private readonly ILessonRepository _lessons;
  private readonly IMaterialRepository _materials;
  private readonly IGradeRepository _grades;

  public TeacherDashboardService(
    ITeacherRepository teachers,
    IGroupRepository groups,
    ILessonRepository lessons,
    IMaterialRepository materials,
    IGradeRepository grades
  )
  {
    _teachers = teachers;
    _groups = groups;
    _lessons = lessons;
    _materials = materials;
    _grades = grades;
  }

  public async Task<Result<TeacherDashboardResponse>> GetAsync(int teacherId, CancellationToken ct)
  {
    _ =
      await _teachers.GetByIdAsync(teacherId, ct)
      ?? throw new NotFoundException("Teacher", teacherId);

    var groups = await _groups
      .GetAll()
      .Include(g => g.GroupStudents)
      .Where(g => g.TeacherId == teacherId)
      .OrderByDescending(g => g.CreatedAt)
      .ToListAsync(ct);

    var groupIds = groups.Select(g => g.Id).ToList();

    var lessons = await _lessons
      .GetAll()
      .Where(l => groupIds.Contains(l.GroupId))
      .Include(l => l.Group)
      .OrderByDescending(l => l.LessonDate)
      .ToListAsync(ct);

    var lessonIds = lessons.Select(l => l.Id).ToList();

    var totalStudents = groups.Sum(g => g.GroupStudents.Count);
    var upcoming = lessons.Count(l => l.LessonDate >= DateTime.UtcNow);

    var allGrades = await _grades
      .GetAll()
      .Where(g => lessonIds.Contains(g.LessonId))
      .Select(g => g.LessonId)
      .Distinct()
      .ToListAsync(ct);
    var gradedLessonIds = new HashSet<int>(allGrades);
    var pendingGrades = lessons.Count(l => !gradedLessonIds.Contains(l.Id));

    var materials = await _materials.GetAll().Where(m => m.TeacherId == teacherId).CountAsync(ct);

    var now = DateTime.UtcNow;
    var daysFromMonday = ((int)now.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
    var weekStart = now.Date.AddDays(-daysFromMonday);
    var weekEnd = weekStart.AddDays(7).AddTicks(-1);

    var weeklyLessons = lessons.Where(l => l.LessonDate >= weekStart && l.LessonDate <= weekEnd).ToList();
    var weeklyCompleted = weeklyLessons.Count(l => l.Status == LessonStatus.Completed);
    var weeklyTotal = weeklyLessons.Count;

    var recentGroups = groups
      .Take(5)
      .Select(g => new TeacherGroupItem(g.Id, g.Name, g.GroupStudents.Count))
      .ToList();

    var recentLessons = lessons
      .Take(10)
      .Select(l => new TeacherLessonItem(
        l.Id,
        l.Group?.Name,
        l.LessonDate,
        l.Topic ?? "",
        l.Status.ToString().ToLower()
      ))
      .ToList();

    return Result<TeacherDashboardResponse>.Success(
      new TeacherDashboardResponse(
        groups.Count,
        totalStudents,
        upcoming,
        materials,
        pendingGrades,
        weeklyCompleted,
        weeklyTotal,
        recentGroups,
        recentLessons
      )
    );
  }

  public async Task<Result<List<WeeklyScheduleItem>>> GetWeeklyScheduleAsync(int teacherId, CancellationToken ct)
  {
    _ =
      await _teachers.GetByIdAsync(teacherId, ct)
      ?? throw new NotFoundException("Teacher", teacherId);

    var groups = await _groups
      .GetAll()
      .Where(g => g.TeacherId == teacherId)
      .ToListAsync(ct);

    var groupIds = groups.Select(g => g.Id).ToList();

    var now = DateTime.UtcNow;
    var daysFromMonday = ((int)now.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
    var weekStart = now.Date.AddDays(-daysFromMonday);
    var weekEnd = weekStart.AddDays(7).AddTicks(-1);

    var lessons = await _lessons
      .GetAll()
      .Where(l => groupIds.Contains(l.GroupId) && l.LessonDate >= weekStart && l.LessonDate <= weekEnd)
      .Include(l => l.Group)
      .OrderBy(l => l.LessonDate)
      .ToListAsync(ct);

    var items = lessons
      .Select(l => new WeeklyScheduleItem(
        l.Id,
        l.GroupId,
        l.Group?.Name ?? "",
        l.Topic ?? "",
        l.LessonDate,
        ((int)l.LessonDate.DayOfWeek - 1 + 7) % 7,
        l.LessonDate.Hour
      ))
      .ToList();

    return Result<List<WeeklyScheduleItem>>.Success(items);
  }
}
