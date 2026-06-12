using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Services;

public class DashboardService : IDashboardService
{
  private readonly ICourseRepository _courses;
  private readonly IGroupRepository _groups;
  private readonly ITeacherRepository _teachers;
  private readonly IStudentRepository _students;
  private readonly ILessonRepository _lessons;

  public DashboardService(
    ICourseRepository courses,
    IGroupRepository groups,
    ITeacherRepository teachers,
    IStudentRepository students,
    ILessonRepository lessons
  )
  {
    _courses = courses;
    _groups = groups;
    _teachers = teachers;
    _students = students;
    _lessons = lessons;
  }

  public async Task<Result<DashboardResponse>> GetAsync(CancellationToken ct)
  {
    var courseCount = await _courses.CountAsync(ct: ct);
    var groupCount = await _groups.CountAsync(ct: ct);
    var teacherCount = await _teachers.CountAsync(ct: ct);
    var studentCount = await _students.CountAsync(ct: ct);

    var allGroups = await _groups
      .GetAll()
      .Include(g => g.Course)
      .OrderByDescending(g => g.CreatedAt)
      .Take(5)
      .ToListAsync(ct);

    var recentGroups = allGroups
      .Select(g => new RecentGroupItem(
        g.Name,
        g.Course.Name,
        g.Status == Domain.Enums.GroupStatus.Active
      ))
      .ToList();

    var recentLessons = await _lessons
      .GetAll()
      .Include(l => l.Group)
      .OrderByDescending(l => l.LessonDate)
      .Take(5)
      .ToListAsync(ct);

    var recentLessonItems = recentLessons
      .Select(l => new RecentLessonItem($"{l.Group?.Name} - {l.Topic}", l.LessonDate))
      .ToList();

    return Result<DashboardResponse>.Success(
      new DashboardResponse(
        courseCount,
        groupCount,
        teacherCount,
        studentCount,
        recentGroups,
        recentLessonItems
      )
    );
  }
}
