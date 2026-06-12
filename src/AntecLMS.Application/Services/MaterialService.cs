using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Services;

public class MaterialService : IMaterialService
{
  private readonly IMaterialRepository _materials;
  private readonly ILessonRepository _lessons;
  private readonly IGroupRepository _groups;
  private readonly IUnitOfWork _uow;

  public MaterialService(
    IMaterialRepository materials,
    ILessonRepository lessons,
    IGroupRepository groups,
    IUnitOfWork uow
  )
  {
    _materials = materials;
    _lessons = lessons;
    _groups = groups;
    _uow = uow;
  }

  public async Task<Result<List<MaterialItem>>> GetByGroupAsync(int groupId, CancellationToken ct)
  {
    _ = await _groups.GetByIdAsync(groupId, ct) ?? throw new NotFoundException("Group", groupId);

    var items = await _materials
      .GetAll()
      .Where(m => m.GroupId == groupId)
      .OrderByDescending(m => m.CreatedAt)
      .ToListAsync(ct);

    var data = items
      .Select(m => new MaterialItem(
        m.Id,
        m.LessonId,
        m.Lesson?.Topic,
        m.Title,
        m.Type,
        m.Url,
        m.FilePath,
        m.Description,
        m.CreatedAt
      ))
      .ToList();

    return Result<List<MaterialItem>>.Success(data);
  }

  public async Task<Result<List<LessonMaterialItem>>> GetByLessonAsync(
    int lessonId,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(lessonId, ct) ?? throw new NotFoundException("Lesson", lessonId);

    var items = await _materials
      .GetAll()
      .Where(m => m.LessonId == lessonId)
      .OrderByDescending(m => m.CreatedAt)
      .ToListAsync(ct);

    var data = items
      .Select(m => new LessonMaterialItem(
        m.Id,
        m.Title,
        m.Type,
        m.Url,
        m.FilePath,
        m.Description,
        m.CreatedAt
      ))
      .ToList();

    return Result<List<LessonMaterialItem>>.Success(data);
  }

  public async Task<Result<MaterialResponse>> CreateAsync(
    CreateMaterialDto dto,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(dto.LessonId, ct)
      ?? throw new NotFoundException("Lesson", dto.LessonId);
    _ =
      await _groups.GetByIdAsync(dto.GroupId, ct)
      ?? throw new NotFoundException("Group", dto.GroupId);

    var material = Material.Create(
      dto.LessonId,
      dto.GroupId,
      dto.TeacherId,
      dto.Title,
      dto.Type,
      dto.Url,
      dto.FilePath,
      dto.Description
    );

    await _materials.AddAsync(material, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<MaterialResponse>.Success(
      new MaterialResponse(
        material.Id,
        material.LessonId,
        material.GroupId,
        material.TeacherId,
        material.Title,
        material.Type,
        material.Url,
        material.FilePath,
        material.Description,
        material.CreatedAt
      ),
      201
    );
  }

  public async Task<Result> DeleteAsync(int id, CancellationToken ct)
  {
    var material =
      await _materials.GetByIdAsync(id, ct) ?? throw new NotFoundException("Material", id);
    _materials.Remove(material);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
