using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;

namespace AntecLMS.Application.Services;

public class StudentPortalService : IStudentPortalService
{
  private readonly IStudentRepository _students;
  private readonly ILessonRepository _lessons;
  private readonly IAttendanceRepository _attendance;
  private readonly IGradeRepository _grades;
  private readonly IMaterialRepository _materials;
  private readonly IUserRepository _users;
  private readonly ICurrentUserService _currentUser;
  private readonly IPasswordHasher _hasher;
  private readonly IUnitOfWork _uow;

  public StudentPortalService(
    IStudentRepository students,
    ILessonRepository lessons,
    IAttendanceRepository attendance,
    IGradeRepository grades,
    IMaterialRepository materials,
    IUserRepository users,
    ICurrentUserService currentUser,
    IPasswordHasher hasher,
    IUnitOfWork uow
  )
  {
    _students = students;
    _lessons = lessons;
    _attendance = attendance;
    _grades = grades;
    _materials = materials;
    _users = users;
    _currentUser = currentUser;
    _hasher = hasher;
    _uow = uow;
  }

  public async Task<Result<MyDashboardResponse>> GetMyDashboardAsync(CancellationToken ct)
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);
    if (student is null)
      return Result<MyDashboardResponse>.Failure("Tələbə tapılmadı.", 404);

    var activeGroup = student
      .GroupStudents.Where(gs => gs.Status == UserStatus.Active)
      .Select(gs => gs.Group)
      .FirstOrDefault();

    var groupInfo = activeGroup is null
      ? null
      : new MyGroupInfo(activeGroup.Id, activeGroup.Name, activeGroup.Status.ToString().ToLower());

    var lessons = await _lessons.GetByStudentUserIdAsync(_currentUser.UserId, ct);
    var recentLessons = lessons
      .Take(5)
      .Select(l => new MyRecentLesson(l.Id, l.Topic ?? "", l.LessonDate, l.Materials.Count))
      .ToList();

    var grades = await _grades.GetByStudentAsync(student.Id, ct);
    var recentGrades = grades
      .Take(5)
      .Select(g => new MyRecentGrade(g.Id, g.Lesson?.Topic ?? "", g.Score, g.MaxScore))
      .ToList();

    var attendances = await _attendance.GetByStudentAsync(student.Id, ct);

    var summary = new MyAttendanceSummary(
      Total: attendances.Count,
      Present: attendances.Count(a => a.Status == AttendanceStatus.Present),
      Absent: attendances.Count(a =>
        a.Status == AttendanceStatus.AbsentExcused || a.Status == AttendanceStatus.AbsentUnexcused
      ),
      Late: attendances.Count(a => a.Status == AttendanceStatus.Late)
    );

    // 1. Lab və Modul ortalamalarını alırıq
    double? labAvg = grades
      .Where(g => g.Category == GradeCategory.Lab)
      .Select(g => (double?)g.Score)
      .Average();

    double? modulAvg = grades
      .Where(g => g.Category == GradeCategory.Modul)
      .Select(g => (double?)g.Score)
      .Average();

    // 2. Final qiymətini alırıq (yoxdursa 0)
    double final = grades.FirstOrDefault(g => g.Category == GradeCategory.Final)?.Score ?? 0;

    // 3. Yekun qiyməti hesablayırıq
    // ?? 0 operatoru - əgər orta qiymət tapılmayıbsa (null-dursa), onu 0 kimi qəbul edir.
    double finalGrade = ((0.5 * (labAvg ?? 0) + 0.5 * (modulAvg ?? 0)) * 0.6) + (final * 0.4);

    bool isEligible = summary.Total > 0 && ((double)summary.Absent / summary.Total) <= 0.25;

    return Result<MyDashboardResponse>.Success(
      new MyDashboardResponse(
        groupInfo,
        recentLessons,
        recentGrades,
        summary,
        finalGrade,
        isEligible
      )
    );
  }

  public async Task<Result<List<MyLessonItem>>> GetMyLessonsAsync(CancellationToken ct)
  {
    var lessons = await _lessons.GetByStudentUserIdAsync(_currentUser.UserId, ct);

    var data = lessons
      .Select(l => new MyLessonItem(
        l.Id,
        l.Topic ?? "",
        l.Note,
        l.LessonDate,
        l.Group?.Name ?? "",
        l.Materials.Select(m => new MyMaterialRef(
            m.Id,
            m.Title,
            m.Description,
            m.Type ?? "",
            m.Url,
            m.FilePath
          ))
          .ToList()
      ))
      .ToList();

    return Result<List<MyLessonItem>>.Success(data);
  }

  public async Task<Result<List<MyGroupDetail>>> GetMyGroupsAsync(CancellationToken ct)
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);
    if (student is null)
      return Result<List<MyGroupDetail>>.Failure("Tələbə tapılmadı.", 404);

    var activeGroups = student
      .GroupStudents.Where(gs => gs.Status == UserStatus.Active)
      .Select(gs => gs.Group)
      .ToList();

    var grades = await _grades.GetByStudentAsync(student.Id, ct);

    var data = new List<MyGroupDetail>();
    foreach (var group in activeGroups)
    {
      var groupLessons = await _lessons.GetByGroupAsync(group.Id, ct);
      var groupLessonIds = groupLessons.Select(l => l.Id).ToHashSet();

      var avgGrade = grades
        .Where(g => groupLessonIds.Contains(g.LessonId))
        .Select(g => (double)g.Score)
        .DefaultIfEmpty()
        .Average();

      data.Add(
        new MyGroupDetail(
          group.Id,
          group.Name,
          groupLessons.Count,
          Math.Round(avgGrade, 2),
          group.Status.ToString().ToLower()
        )
      );
    }

    return Result<List<MyGroupDetail>>.Success(data);
  }

  public async Task<Result<AttendanceJournalResponse>> GetAttendanceJournalAsync(
    DateTime? startDate,
    DateTime? endDate,
    CancellationToken ct
  )
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);
    if (student is null)
      return Result<AttendanceJournalResponse>.Failure("Tələbə tapılmadı.", 404);

    // Bütün davamiyyəti çək
    var items = await _attendance.GetByStudentAsync(student.Id, ct);

    // Tarixə görə filterləmə
    var filtered = items
      .Where(a =>
        (!startDate.HasValue || a.Lesson.LessonDate >= startDate)
        && (!endDate.HasValue || a.Lesson.LessonDate <= endDate)
      )
      .ToList();

    // Statistikanı hesabla
    var present = filtered.Count(a => a.Status == AttendanceStatus.Present);
    var excused = filtered.Count(a => a.Status == AttendanceStatus.AbsentExcused);
    var absent = filtered.Count(a => a.Status == AttendanceStatus.AbsentUnexcused);
    var late = filtered.Count(a => a.Status == AttendanceStatus.Late);

    var total = filtered.Count;
    var percentage = total > 0 ? (double)present / total * 100 : 0;

    // Cavabı formalaşdır
    var data = filtered
      .Select(a => new AttendanceItem(
        a.Id,
        a.StudentId,
        $"{student.User.Name} {student.User.Surname}",
        a.Status.ToString(),
        a.MinutesLate,
        a.Reason,
        a.TeacherNote,
        a.CreatedAt
      ))
      .ToList();

    return Result<AttendanceJournalResponse>.Success(
      new AttendanceJournalResponse(data, present, excused, absent, late, percentage)
    );
  }

  public async Task<Result<GradeJournalResponse>> GetGradeJournalAsync(
    DateTime? startDate,
    DateTime? endDate,
    CancellationToken ct
  )
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);
    if (student is null)
      return Result<GradeJournalResponse>.Failure("Tələbə tapılmadı.", 404);

    var grades = await _grades.GetByStudentAsync(student.Id, ct);
    var attendances = await _attendance.GetByStudentAsync(student.Id, ct);

    var filtered = grades
      .Where(g =>
        (!startDate.HasValue || g.Lesson.LessonDate >= startDate)
        && (!endDate.HasValue || g.Lesson.LessonDate <= endDate)
      )
      .ToList();

    var items = filtered
      .Select(g => new MyGradeItem(
        g.Id,
        g.Lesson.Topic ?? "",
        g.Lesson.LessonDate,
        g.Score,
        g.MaxScore,
        g.TeacherNote
      ))
      .ToList();

    var totalLessons = attendances.Count;
    var absents = attendances.Count(a =>
      a.Status == AttendanceStatus.AbsentUnexcused || a.Status == AttendanceStatus.AbsentExcused
    );
    bool canAttendFinal = (totalLessons == 0) || ((double)absents / totalLessons <= 0.25);

    var avgPercentage = items.Any() ? items.Average(i => i.Percentage) : 0;
    var maxScore = items.Any() ? items.Max(i => i.Score) : 0;
    var minScore = items.Any() ? items.Min(i => i.Score) : 0;

    // 3. İndi return edərkən hamısı tanınacaq
    return Result<GradeJournalResponse>.Success(
      new GradeJournalResponse(
        items,
        totalLessons,
        Math.Round(avgPercentage, 2),
        maxScore,
        minScore,
        canAttendFinal,
        null
      )
    );
  }

  public async Task<Result<List<MaterialItem>>> GetMyMaterialsAsync(
    string? type,
    CancellationToken ct
  )
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);
    if (student is null)
      return Result<List<MaterialItem>>.Failure("Tələbə tapılmadı.", 404);

    // Bütün materialları qruplar üzrə çək
    var materials = await _materials.GetByStudentGroupsAsync(student.Id, ct);

    // Tipə görə filterləmə (Frontend "Hamısı" göndərirsə, hamısını qaytar)
    if (!string.IsNullOrEmpty(type) && type != "Hamısı")
    {
      materials = materials
        .Where(m => m.Type != null && m.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
        .ToList();
    }

    var data = materials
      .Select(m => new MaterialItem(
        m.Id,
        m.LessonId,
        m.Lesson?.Topic ?? "Adsız dərs", // Burada '?' işarəsi əlavə etdik (null-check)
        m.Title,
        m.Type ?? "",
        m.Url,
        m.FilePath,
        m.Description,
        m.CreatedAt
      ))
      .ToList();

    return Result<List<MaterialItem>>.Success(data);
  }

  public async Task<Result<MyProfileResponse>> GetMyProfileAsync(CancellationToken ct)
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);

    // Null yoxlanışını daha təmiz formatda
    if (student?.User is null)
      return Result<MyProfileResponse>.Failure(
        "Tələbə və ya istifadəçi məlumatları tapılmadı.",
        404
      );

    // Məlumatları birbaşa konstruktora ötürürük
    var profile = new MyProfileResponse(
      student.Id,
      student.User.Name,
      student.User.Surname,
      student.User.Email,
      student.User.Phone,
      student.BirthDate,
      student.Note,
      student.Status.ToString().ToLower()
    );

    return Result<MyProfileResponse>.Success(profile);
  }

  public async Task<Result> ChangePasswordAsync(
    string currentPassword,
    string newPassword,
    CancellationToken ct
  )
  {
    var user = await _users.GetByIdAsync(_currentUser.UserId, ct);

    if (user is null)
      return Result.Failure("İstifadəçi tapılmadı.", 404);

    // Şifrə yoxlanışı
    if (!_hasher.Verify(currentPassword, user.Password))
      return Result.Failure("Mövcud şifrə yanlışdır.", 422);

    // Şifrəni hash-ləyib dəyişirik
    var hashedPassword = _hasher.Hash(newPassword);
    user.ChangePassword(hashedPassword);

    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}
