using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Materials.Queries.GetGroupMaterials;

public class GetGroupMaterialsHandler
  : IRequestHandler<GetGroupMaterialsQuery, Result<List<MaterialItem>>>
{
  private readonly IMaterialRepository _materials;
  private readonly IGroupRepository _groups;

  public GetGroupMaterialsHandler(IMaterialRepository materials, IGroupRepository groups)
  {
    _materials = materials;
    _groups = groups;
  }

  public async Task<Result<List<MaterialItem>>> Handle(
    GetGroupMaterialsQuery request,
    CancellationToken ct
  )
  {
    _ =
      await _groups.GetByIdAsync(request.GroupId, ct)
      ?? throw new NotFoundException("Group", request.GroupId);

    var items = await _materials
      .GetAll()
      .Where(m => m.GroupId == request.GroupId)
      .Include(m => m.Lesson)
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
}
