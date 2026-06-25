using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class MaterialsController : BaseApiController
{
  private readonly IMaterialService _materials;
  private readonly IFileStorageService _fileStorage;

  public MaterialsController(IMaterialService materials, IFileStorageService fileStorage)
  {
    _materials = materials;
    _fileStorage = fileStorage;
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

  [HttpPost("upload")]
  [RequestSizeLimit(52_428_800)]
  public async Task<IActionResult> Upload(
    [FromForm] int lessonId,
    [FromForm] int groupId,
    [FromForm] int teacherId,
    [FromForm] string title,
    [FromForm] string type,
    [FromForm] string? description,
    IFormFile? file,
    CancellationToken ct
  )
  {
    string? filePath = null;

    if (file is not null && file.Length > 0)
    {
      await using var stream = file.OpenReadStream();
      filePath = await _fileStorage.SaveFileAsync(stream, file.FileName, ct);
    }

    var dto = new CreateMaterialDto(
      lessonId,
      groupId,
      teacherId,
      title,
      type,
      Url: null,
      FilePath: filePath,
      description
    );

    var result = await _materials.CreateAsync(dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Material uğurla əlavə edildi.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateMaterialDto dto,
    CancellationToken ct
  )
  {
    var result = await _materials.UpdateAsync(id, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Material uğurla yeniləndi.", data = result.Data });
  }

  [HttpPut("{id:int}/upload")]
  [RequestSizeLimit(52_428_800)]
  public async Task<IActionResult> UpdateWithFile(
    int id,
    [FromForm] string title,
    [FromForm] string type,
    [FromForm] string? description,
    IFormFile? file,
    CancellationToken ct
  )
  {
    string? filePath = null;

    if (file is not null && file.Length > 0)
    {
      await using var stream = file.OpenReadStream();
      filePath = await _fileStorage.SaveFileAsync(stream, file.FileName, ct);
    }

    var dto = new UpdateMaterialDto(title, type, Url: null, FilePath: filePath, description);

    var result = await _materials.UpdateAsync(id, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Material uğurla yeniləndi.", data = result.Data });
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
