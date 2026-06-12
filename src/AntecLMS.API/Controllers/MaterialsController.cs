using AntecLMS.Application.Features.Materials.Commands.CreateMaterial;
using AntecLMS.Application.Features.Materials.Commands.DeleteMaterial;
using AntecLMS.Application.Features.Materials.Queries.GetGroupMaterials;
using AntecLMS.Application.Features.Materials.Queries.GetLessonMaterials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class MaterialsController : BaseApiController
{
  [HttpGet("group/{groupId:int}")]
  public async Task<IActionResult> GetByGroup(int groupId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetGroupMaterialsQuery(groupId), ct);
    return ToResponse(result);
  }

  [HttpGet("lesson/{lessonId:int}")]
  public async Task<IActionResult> GetByLesson(int lessonId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetLessonMaterialsQuery(lessonId), ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create(
    [FromBody] CreateMaterialCommand command,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(command, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Material uğurla əlavə edildi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await Mediator.Send(new DeleteMaterialCommand(id), ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Material silindi." });
  }
}
