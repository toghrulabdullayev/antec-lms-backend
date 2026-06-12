using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Application.Features.Materials.Queries.GetLessonMaterials;

public class GetLessonMaterialsHandler
  : IRequestHandler<GetLessonMaterialsQuery, Result<List<LessonMaterialItem>>>
{
  private readonly IMaterialRepository _materials;
  private readonly ILessonRepository _lessons;

  public GetLessonMaterialsHandler(IMaterialRepository materials, ILessonRepository lessons)
  {
    _materials = materials;
    _lessons = lessons;
  }

  public async Task<Result<List<LessonMaterialItem>>> Handle(
    GetLessonMaterialsQuery request,
    CancellationToken ct
  )
  {
    _ =
      await _lessons.GetByIdAsync(request.LessonId, ct)
      ?? throw new NotFoundException("Lesson", request.LessonId);

    var items = await _materials
      .GetAll()
      .Where(m => m.LessonId == request.LessonId)
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
}
