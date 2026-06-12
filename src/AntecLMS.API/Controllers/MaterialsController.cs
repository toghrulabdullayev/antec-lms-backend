using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class MaterialsController : BaseApiController
{
  private readonly IMaterialService _materials;

  public MaterialsController(IMaterialService materials)
  {
    _materials = materials;
  }

  [HttpGet("group/{groupId:int}")]
  public async Task<IActionResult> GetByGroup(int groupId, CancellationToken ct)
  {
    var result = await _materials.GetByGroupAsync(groupId, ct);
    return ToResponse(result);
  }

  [HttpGet("lesson/{lessonId:int}")]
  public async Task<IActionResult> GetByLesson(int lessonId, CancellationToken ct)
  {
    var result = await _materials.GetByLessonAsync(lessonId, ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateMaterialDto dto, CancellationToken ct)
  {
    var result = await _materials.CreateAsync(dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Material uğurla əlavə edildi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await _materials.DeleteAsync(id, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Material silindi." });
  }
}
