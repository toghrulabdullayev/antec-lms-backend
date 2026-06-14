using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
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
        a.Status == AttendanceStatus.Excused || a.Status == AttendanceStatus.Absent
      ),
      Late: attendances.Count(a => a.Status == AttendanceStatus.Late)
    );

    return Result<MyDashboardResponse>.Success(
      new MyDashboardResponse(groupInfo, recentLessons, recentGrades, summary)
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
            m.FilePath
          ))
          .ToList()
      ))
      .ToList();

    return Result<List<MyLessonItem>>.Success(data);
  }

  public async Task<Result<List<MyAttendanceItem>>> GetMyAttendanceAsync(CancellationToken ct)
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);
    if (student is null)
      return Result<List<MyAttendanceItem>>.Failure("Tələbə tapılmadı.", 404);

    var items = await _attendance.GetByStudentAsync(student.Id, ct);

    var data = items
      .Select(a => new MyAttendanceItem(
        a.Id,
        a.Lesson.LessonDate,
        a.Lesson.Topic ?? "",
        a.Status.ToString().ToLower(),
        a.MinutesLate,
        a.Reason
      ))
      .ToList();

    return Result<List<MyAttendanceItem>>.Success(data);
  }

  public async Task<Result<List<MyGradeItem>>> GetMyGradesAsync(CancellationToken ct)
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);
    if (student is null)
      return Result<List<MyGradeItem>>.Failure("Tələbə tapılmadı.", 404);

    var items = await _grades.GetByStudentAsync(student.Id, ct);

    var data = items
      .Select(g => new MyGradeItem(
        g.Id,
        g.Lesson.Topic ?? "",
        g.Lesson.LessonDate,
        g.Score,
        g.MaxScore,
        g.TeacherNote
      ))
      .ToList();

    return Result<List<MyGradeItem>>.Success(data);
  }

  public async Task<Result<List<MyMaterialDetail>>> GetMyMaterialsAsync(CancellationToken ct)
  {
    var items = await _materials.GetByStudentUserIdAsync(_currentUser.UserId, ct);

    var data = items
      .Select(m => new MyMaterialDetail(
        m.Id,
        m.Title,
        m.Description,
        m.Type ?? "",
        m.FilePath,
        m.Lesson.Topic ?? "",
        m.Lesson.LessonDate
      ))
      .ToList();

    return Result<List<MyMaterialDetail>>.Success(data);
  }

  public async Task<Result<MyProfileResponse>> GetMyProfileAsync(CancellationToken ct)
  {
    var student = await _students.GetByUserIdAsync(_currentUser.UserId, ct);
    if (student is null)
      return Result<MyProfileResponse>.Failure("Tələbə tapılmadı.", 404);

    return Result<MyProfileResponse>.Success(
      new MyProfileResponse(
        student.Id,
        student.User.Name,
        student.User.Surname,
        student.User.Email,
        student.User.Phone,
        student.BirthDate,
        student.Note,
        student.Status.ToString().ToLower()
      )
    );
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

    if (!_hasher.Verify(currentPassword, user.Password))
      return Result.Failure("Mövcud şifrə yanlışdır.", 422);

    user.ChangePassword(_hasher.Hash(newPassword));
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}
