using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;

namespace AntecLMS.Application.Services;

public class GroupService : IGroupService
{
  private readonly IGroupRepository _groups;
  private readonly ICourseRepository _courses;
  private readonly ITeacherRepository _teachers;
  private readonly IStudentRepository _students;
  private readonly IGroupScheduleRepository _schedules;
  private readonly IUnitOfWork _uow;

  public GroupService(
    IGroupRepository groups,
    ICourseRepository courses,
    ITeacherRepository teachers,
    IStudentRepository students,
    IGroupScheduleRepository schedules,
    IUnitOfWork uow
  )
  {
    _groups = groups;
    _courses = courses;
    _teachers = teachers;
    _students = students;
    _schedules = schedules;
    _uow = uow;
  }

  public async Task<Result<PagedResult<GroupListItem>>> GetAllAsync(
    int? courseId,
    int? teacherId,
    string? status,
    int page,
    int perPage,
    CancellationToken ct
  )
  {
    GroupStatus? statusEnum = status is null ? null : Enum.Parse<GroupStatus>(status, true);
    var (items, total) = await _groups.GetPagedAsync(
      courseId,
      teacherId,
      statusEnum,
      page,
      perPage,
      ct
    );

    var data = items
      .Select(g => new GroupListItem(
        g.Id,
        g.Name,
        new CourseRef(g.Course.Id, g.Course.Name),
        new TeacherRef(g.Teacher.Id, g.Teacher.User.Name, g.Teacher.User.Surname),
        g.GroupStudents.Count(gs => gs.Status == UserStatus.Active),
        g.StartDate,
        g.EndDate,
        g.Status.ToString().ToLower()
      ))
      .ToList();

    return Result<PagedResult<GroupListItem>>.Success(
      new PagedResult<GroupListItem>
      {
        Data = data,
        Total = total,
        Page = page,
        PerPage = perPage,
      }
    );
  }

  public async Task<Result<GroupDetailResponse>> GetByIdAsync(int id, CancellationToken ct)
  {
    var group =
      await _groups.GetWithDetailsAsync(id, ct) ?? throw new NotFoundException("Group", id);

    var students = group
      .GroupStudents.Where(gs => gs.Status == UserStatus.Active)
      .Select(gs => new StudentInGroup(
        gs.Student.Id,
        gs.Student.User.Name,
        gs.Student.User.Surname,
        gs.Student.Status.ToString().ToLower()
      ))
      .ToList();

    var scheduleItems = (await _schedules.GetByGroupAsync(id, ct))
      .Select(s => new GroupScheduleItem(
        s.DayOfWeek.ToString(),
        s.StartTime.ToString(),
        s.EndTime.ToString(),
        s.RoomOrNote
      ))
      .ToList();

    return Result<GroupDetailResponse>.Success(
      new GroupDetailResponse(
        group.Id,
        group.Name,
        new CourseInfo(group.Course.Id, group.Course.Name),
        new TeacherInfo(group.Teacher.Id, group.Teacher.User.Name, group.Teacher.User.Surname),
        group.StartDate,
        group.EndDate,
        group.Status.ToString().ToLower(),
        students,
        students.Count,
        scheduleItems
      )
    );
  }

  public async Task<Result<GroupResponse>> CreateAsync(CreateGroupDto dto, CancellationToken ct)
  {
    var course = await _courses.GetByIdAsync(dto.CourseId, ct);
    if (course is null)
      return Result<GroupResponse>.Failure("Kurs tapılmadı.", 404);

    var teacher = await _teachers.GetByIdAsync(dto.TeacherId, ct);
    if (teacher is null)
      return Result<GroupResponse>.Failure("Müəllim tapılmadı.", 404);

    var status = Enum.Parse<GroupStatus>(dto.Status, true);
    var group = Group.Create(
      dto.Name,
      dto.CourseId,
      dto.TeacherId,
      dto.StartDate,
      dto.EndDate,
      status
    );

    await _groups.AddAsync(group, ct);

    if (dto.Schedules != null)
    {
      foreach (var s in dto.Schedules)
      {
        if (!Enum.TryParse<DayOfWeek>(s.DayOfWeek, true, out var day))
          return Result<GroupResponse>.Failure($"Yanlış gün formatı: '{s.DayOfWeek}'", 400);

        if (!TimeOnly.TryParse(s.StartTime, out var start))
          return Result<GroupResponse>.Failure($"Yanlış başlama vaxtı formatı: '{s.StartTime}'", 400);

        if (!TimeOnly.TryParse(s.EndTime, out var end))
          return Result<GroupResponse>.Failure($"Yanlış bitmə vaxtı formatı: '{s.EndTime}'", 400);

        if (end <= start)
          return Result<GroupResponse>.Failure("Bitmə vaxtı başlama vaxtından sonra olmalıdır.", 400);

        var schedule = GroupSchedule.Create(group.Id, day, start, end, s.RoomOrNote);
        await _schedules.AddAsync(schedule, ct);
      }
    }

    await _uow.SaveChangesAsync(ct);

    return Result<GroupResponse>.Success(
      new GroupResponse(
        group.Id,
        group.Name,
        group.CourseId,
        group.TeacherId,
        group.StartDate,
        group.Status.ToString().ToLower()
      ),
      201
    );
  }

  public async Task<Result<GroupResponse>> UpdateAsync(
    int id,
    UpdateGroupDto dto,
    CancellationToken ct
  )
  {
    var group = await _groups.GetByIdAsync(id, ct) ?? throw new NotFoundException("Group", id);

    var course = await _courses.GetByIdAsync(dto.CourseId, ct);
    if (course is null)
      return Result<GroupResponse>.Failure("Kurs tapılmadı.", 404);

    var teacher = await _teachers.GetByIdAsync(dto.TeacherId, ct);
    if (teacher is null)
      return Result<GroupResponse>.Failure("Müəllim tapılmadı.", 404);

    var status = Enum.Parse<GroupStatus>(dto.Status, true);
    group.Update(dto.Name, dto.CourseId, dto.TeacherId, dto.StartDate, dto.EndDate, status);

    _groups.Update(group);

    if (dto.Schedules != null)
    {
      var newSchedules = new List<GroupSchedule>();
      foreach (var s in dto.Schedules)
      {
        if (!Enum.TryParse<DayOfWeek>(s.DayOfWeek, true, out var day))
          return Result<GroupResponse>.Failure($"Yanlış gün formatı: '{s.DayOfWeek}'", 400);

        if (!TimeOnly.TryParse(s.StartTime, out var start))
          return Result<GroupResponse>.Failure($"Yanlış başlama vaxtı formatı: '{s.StartTime}'", 400);

        if (!TimeOnly.TryParse(s.EndTime, out var end))
          return Result<GroupResponse>.Failure($"Yanlış bitmə vaxtı formatı: '{s.EndTime}'", 400);

        if (end <= start)
          return Result<GroupResponse>.Failure("Bitmə vaxtı başlama vaxtından sonra olmalıdır.", 400);

        newSchedules.Add(GroupSchedule.Create(group.Id, day, start, end, s.RoomOrNote));
      }

      await _schedules.ReplaceForGroupAsync(group.Id, newSchedules, ct);
    }

    await _uow.SaveChangesAsync(ct);

    return Result<GroupResponse>.Success(
      new GroupResponse(
        group.Id,
        group.Name,
        group.CourseId,
        group.TeacherId,
        group.StartDate,
        group.Status.ToString().ToLower()
      )
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var group = await _groups.GetByIdAsync(id, ct) ?? throw new NotFoundException("Group", id);
    _groups.Remove(group);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }

  public async Task<Result> AddStudentAsync(int groupId, int studentId, CancellationToken ct)
  {
    var group = await _groups.GetByIdAsync(groupId, ct);
    if (group is null)
      return Result.Failure("Qrup tapılmadı.", 404);

    var student = await _students.GetByIdAsync(studentId, ct);
    if (student is null)
      return Result.Failure("Tələbə tapılmadı.", 404);

    if (await _groups.StudentExistsInGroupAsync(groupId, studentId, ct))
      return Result.Failure("Bu tələbə artıq bu qrupdadır.", 400);

    var gs = new GroupStudent
    {
      GroupId = groupId,
      StudentId = studentId,
      JoinedAt = DateTime.UtcNow,
      Status = UserStatus.Active,
    };

    await _groups.AddStudentAsync(gs, ct);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }

  public async Task<Result> RemoveStudentAsync(int groupId, int studentId, CancellationToken ct)
  {
    var gs = await _groups.GetGroupStudentAsync(groupId, studentId, ct);
    if (gs is null)
      return Result.Failure("Tələbə bu qrupda deyil.", 404);

    gs.Status = UserStatus.Inactive;
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }

  public async Task<Result<List<GroupScheduleItem>>> GetScheduleAsync(int groupId, CancellationToken ct)
  {
    _ = await _groups.GetByIdAsync(groupId, ct) ?? throw new NotFoundException("Group", groupId);

    var items = (await _schedules.GetByGroupAsync(groupId, ct))
      .Select(s => new GroupScheduleItem(
        s.DayOfWeek.ToString(),
        s.StartTime.ToString(),
        s.EndTime.ToString(),
        s.RoomOrNote
      ))
      .ToList();

    return Result<List<GroupScheduleItem>>.Success(items);
  }
}
