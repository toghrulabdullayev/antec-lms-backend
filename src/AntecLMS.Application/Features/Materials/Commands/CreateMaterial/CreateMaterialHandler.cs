using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Materials.Commands.CreateMaterial;

public class CreateMaterialHandler
  : IRequestHandler<CreateMaterialCommand, Result<MaterialResponse>>
{
  private readonly IMaterialRepository _materials;
  private readonly ILessonRepository _lessons;
  private readonly IGroupRepository _groups;
  private readonly ITeacherRepository _teachers;
  private readonly IUnitOfWork _uow;

  public CreateMaterialHandler(
    IMaterialRepository materials,
    ILessonRepository lessons,
    IGroupRepository groups,
    ITeacherRepository teachers,
    IUnitOfWork uow
  )
  {
    _materials = materials;
    _lessons = lessons;
    _groups = groups;
    _teachers = teachers;
    _uow = uow;
  }

  public async Task<Result<MaterialResponse>> Handle(
    CreateMaterialCommand request,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(request.LessonId, ct)
      ?? throw new NotFoundException("Lesson", request.LessonId);
    _ =
      await _groups.GetByIdAsync(request.GroupId, ct)
      ?? throw new NotFoundException("Group", request.GroupId);
    _ =
      await _teachers.GetByIdAsync(request.TeacherId, ct)
      ?? throw new NotFoundException("Teacher", request.TeacherId);

    var material = Material.Create(
      request.LessonId,
      request.GroupId,
      request.TeacherId,
      request.Title,
      request.Type,
      request.Url,
      request.FilePath,
      request.Description
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
}
