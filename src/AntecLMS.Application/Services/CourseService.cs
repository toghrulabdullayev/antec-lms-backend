using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;

namespace AntecLMS.Application.Services;

public class CourseService : ICourseService
{
  private readonly ICourseRepository _courses;
  private readonly IUnitOfWork _uow;

  public CourseService(ICourseRepository courses, IUnitOfWork uow)
  {
    _courses = courses;
    _uow = uow;
  }

  public async Task<Result<PagedResult<CourseListItem>>> GetAllAsync(
    string? status,
    string? search,
    int page,
    int perPage,
    CancellationToken ct
  )
  {
    CourseStatus? statusEnum = status is null ? null : Enum.Parse<CourseStatus>(status, true);
    var (items, total) = await _courses.GetPagedAsync(statusEnum, search, page, perPage, ct);

    var data = items
      .Select(c => new CourseListItem(
        c.Id,
        c.Name,
        c.Description,
        c.Price,
        c.ImageUrl,
        c.Status.ToString().ToLower(),
        c.CreatedAt
      ))
      .ToList();

    return Result<PagedResult<CourseListItem>>.Success(
      new PagedResult<CourseListItem>
      {
        Data = data,
        Total = total,
        Page = page,
        PerPage = perPage,
      }
    );
  }

  public async Task<Result<CourseDetailResponse>> GetByIdAsync(int id, CancellationToken ct)
  {
    var course = await _courses.GetByIdAsync(id, ct) ?? throw new NotFoundException("Course", id);

    return Result<CourseDetailResponse>.Success(
      new CourseDetailResponse(
        course.Id,
        course.Name,
        course.Description,
        course.Price,
        course.ImageUrl,
        course.Status.ToString().ToLower(),
        course.Groups.Count,
        course.CreatedAt
      )
    );
  }

  public async Task<Result<CourseResponse>> CreateAsync(CreateCourseDto dto, CancellationToken ct)
  {
    var status = Enum.Parse<CourseStatus>(dto.Status, true);
    var course = Course.Create(dto.Name, dto.Description, dto.Price, dto.ImageUrl, status);

    await _courses.AddAsync(course, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<CourseResponse>.Success(
      new CourseResponse(
        course.Id,
        course.Name,
        course.Description,
        course.Price,
        course.ImageUrl,
        course.Status.ToString().ToLower(),
        course.CreatedAt
      ),
      201
    );
  }

  public async Task<Result<CourseResponse>> UpdateAsync(
    int id,
    UpdateCourseDto dto,
    CancellationToken ct
  )
  {
    var course = await _courses.GetByIdAsync(id, ct) ?? throw new NotFoundException("Course", id);
    var status = Enum.Parse<CourseStatus>(dto.Status, true);
    course.Update(dto.Name, dto.Description, dto.Price, dto.ImageUrl, status);

    _courses.Update(course);
    await _uow.SaveChangesAsync(ct);

    return Result<CourseResponse>.Success(
      new CourseResponse(
        course.Id,
        course.Name,
        course.Description,
        course.Price,
        course.ImageUrl,
        course.Status.ToString().ToLower(),
        course.CreatedAt
      )
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var course = await _courses.GetByIdAsync(id, ct) ?? throw new NotFoundException("Course", id);

    if (await _courses.HasActiveGroupsAsync(id, ct))
      return Result.Failure("Bu kursla bağlı aktiv qruplar var, silinə bilməz.", 400);

    _courses.Remove(course);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
