using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Services;

public class StudentService : IStudentService
{
  private readonly IUserRepository _users;
  private readonly IStudentRepository _students;
  private readonly IUnitOfWork _uow;
  private readonly IPasswordHasher _hasher;

  public StudentService(
    IUserRepository users,
    IStudentRepository students,
    IUnitOfWork uow,
    IPasswordHasher hasher
  )
  {
    _users = users;
    _students = students;
    _uow = uow;
    _hasher = hasher;
  }

  public async Task<Result<PagedResult<StudentListItem>>> GetAllAsync(
    int? groupId,
    string? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct
  )
  {
    var statusEnum = status is not null ? Enum.Parse<UserStatus>(status, true) : (UserStatus?)null;
    var (items, total) = await _students.GetPagedAsync(
      groupId,
      statusEnum,
      search,
      page,
      perPage,
      ct
    );

    var data = items
      .Select(s => new StudentListItem(
        s.Id,
        s.UserId,
        s.User.Name,
        s.User.Surname,
        s.User.Email,
        s.User.Phone,
        s.BirthDate,
        s.Status.ToString().ToLower(),
        s.GroupStudents.Select(gs => new GroupRefS(gs.Group.Id, gs.Group.Name)).ToList()
      ))
      .ToList();

    return Result<PagedResult<StudentListItem>>.Success(
      new PagedResult<StudentListItem>
      {
        Data = data,
        Total = total,
        Page = page,
        PerPage = perPage,
      }
    );
  }

  public async Task<Result<StudentDetailResponse>> GetByIdAsync(int id, CancellationToken ct)
  {
    var student =
      await _students.GetWithUserAsync(id, ct) ?? throw new NotFoundException("Student", id);

    var groups = student
      .GroupStudents.Select(gs => new GroupRefD(gs.Group.Id, gs.Group.Name))
      .ToList();

    return Result<StudentDetailResponse>.Success(
      new StudentDetailResponse(
        student.Id,
        student.UserId,
        student.User.Name,
        student.User.Surname,
        student.User.Email,
        student.User.Phone,
        student.BirthDate,
        student.Note,
        student.Status.ToString().ToLower(),
        groups
      )
    );
  }

  public async Task<Result<StudentResponse>> CreateAsync(CreateStudentDto dto, CancellationToken ct)
  {
    if (await _users.EmailExistsAsync(dto.Email, ct))
      return Result<StudentResponse>.Failure("Bu email artıq mövcuddur.", 422);

    var status = Enum.Parse<UserStatus>(dto.Status, true);

    var user = User.Create(
      dto.Name,
      dto.Surname,
      dto.Email,
      _hasher.Hash(dto.Password),
      UserRole.Student,
      dto.Phone,
      status
    );

    await _users.AddAsync(user, ct);
    await _uow.SaveChangesAsync(ct);

    var student = Student.Create(user.Id, dto.BirthDate, dto.Note, status);
    await _students.AddAsync(student, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<StudentResponse>.Success(
      new StudentResponse(
        student.Id,
        user.Id,
        user.Name,
        user.Surname,
        user.Email,
        student.Status.ToString().ToLower()
      ),
      201
    );
  }

  public async Task<Result<StudentResponse>> UpdateAsync(
    int id,
    UpdateStudentDto dto,
    CancellationToken ct
  )
  {
    var student =
      await _students.GetAll().Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id, ct)
      ?? throw new NotFoundException("Student", id);

    if (dto.Phone is not null && student.User is not null)
      student.User.Update(student.User.Name, student.User.Surname, dto.Phone, student.User.Status);

    student.Update(dto.Note, Enum.Parse<Domain.Enums.UserStatus>(dto.Status, true));
    _students.Update(student);
    await _uow.SaveChangesAsync(ct);

    return Result<StudentResponse>.Success(
      new StudentResponse(
        student.Id,
        student.UserId,
        student.User?.Name ?? "",
        student.User?.Surname ?? "",
        student.User?.Email ?? "",
        student.Status.ToString().ToLower()
      )
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var student =
      await _students.GetByIdAsync(id, ct) ?? throw new NotFoundException("Student", id);
    _students.Remove(student);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }

  public async Task<Result<List<StudentAttendanceItem>>> GetAttendancesAsync(
    int studentId,
    CancellationToken ct
  )
  {
    var items = await _students
      .GetAll()
      .Where(s => s.Id == studentId)
      .SelectMany(s => s.Attendances)
      .Include(a => a.Lesson)
      .OrderByDescending(a => a.Lesson.LessonDate)
      .Select(a => new StudentAttendanceItem(
        a.Id,
        a.LessonId,
        a.Lesson.Topic + " - " + a.Lesson.LessonDate,
        a.Lesson.LessonDate,
        a.Status.ToString().ToLower(),
        a.MinutesLate,
        a.Reason,
        a.TeacherNote,
        a.CreatedAt
      ))
      .ToListAsync(ct);

    return Result<List<StudentAttendanceItem>>.Success(items);
  }

  public async Task<Result<List<StudentGradeItem>>> GetGradesAsync(
    int studentId,
    CancellationToken ct
  )
  {
    var items = await _students
      .GetAll()
      .Where(s => s.Id == studentId)
      .SelectMany(s => s.Grades)
      .Include(g => g.Lesson)
      .OrderByDescending(g => g.Lesson.LessonDate)
      .Select(g => new StudentGradeItem(
        g.Id,
        g.LessonId,
        g.Lesson.Topic + " - " + g.Lesson.LessonDate,
        g.Lesson.LessonDate,
        g.Score,
        g.MaxScore,
        g.TeacherNote,
        g.CreatedAt
      ))
      .ToListAsync(ct);

    return Result<List<StudentGradeItem>>.Success(items);
  }
}
