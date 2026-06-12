using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class LessonsController : BaseApiController
{
  private readonly ILessonService _lessons;
  private readonly IAttendanceService _attendances;
  private readonly IGradeService _grades;

  public LessonsController(
    ILessonService lessons,
    IAttendanceService attendances,
    IGradeService grades
  )
  {
    _lessons = lessons;
    _attendances = attendances;
    _grades = grades;
  }

  [HttpGet("group/{groupId:int}")]
  public async Task<IActionResult> GetByGroup(int groupId, CancellationToken ct)
  {
    var result = await _lessons.GetByGroupAsync(groupId, ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await _lessons.GetByIdAsync(id, ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateLessonDto dto, CancellationToken ct)
  {
    var result = await _lessons.CreateAsync(dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Dərs uğurla yaradıldı.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateLessonDto dto,
    CancellationToken ct
  )
  {
    var result = await _lessons.UpdateAsync(id, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Dərs uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await _lessons.DeleteAsync(id, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Dərs uğurla silindi." });
  }

  // --- Attendances per lesson ---

  [HttpGet("{lessonId:int}/attendances")]
  public async Task<IActionResult> GetAttendances(int lessonId, CancellationToken ct)
  {
    var result = await _attendances.GetByLessonAsync(lessonId, ct);
    return ToResponse(result);
  }

  [HttpPost("{lessonId:int}/attendances")]
  public async Task<IActionResult> CreateAttendance(
    int lessonId,
    [FromBody] CreateAttendanceDto dto,
    CancellationToken ct
  )
  {
    var result = await _attendances.CreateAsync(lessonId, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Davamiyyət qeyd edildi.", data = result.Data });
  }

  [HttpPut("attendances/{attendanceId:int}")]
  public async Task<IActionResult> UpdateAttendance(
    int attendanceId,
    [FromBody] UpdateAttendanceDto dto,
    CancellationToken ct
  )
  {
    var result = await _attendances.UpdateAsync(attendanceId, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Davamiyyət yeniləndi.", data = result.Data });
  }

  [HttpDelete("attendances/{attendanceId:int}")]
  public async Task<IActionResult> DeleteAttendance(int attendanceId, CancellationToken ct)
  {
    var result = await _attendances.DeleteAsync(attendanceId, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Davamiyyət silindi." });
  }

  // --- Grades per lesson ---

  [HttpGet("{lessonId:int}/grades")]
  public async Task<IActionResult> GetGrades(int lessonId, CancellationToken ct)
  {
    var result = await _grades.GetByLessonAsync(lessonId, ct);
    return ToResponse(result);
  }

  [HttpPost("{lessonId:int}/grades")]
  public async Task<IActionResult> CreateGrade(
    int lessonId,
    [FromBody] CreateGradeDto dto,
    CancellationToken ct
  )
  {
    var result = await _grades.CreateAsync(lessonId, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Qiymət qeyd edildi.", data = result.Data });
  }

  [HttpPut("grades/{gradeId:int}")]
  public async Task<IActionResult> UpdateGrade(
    int gradeId,
    [FromBody] UpdateGradeDto dto,
    CancellationToken ct
  )
  {
    var result = await _grades.UpdateAsync(gradeId, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Qiymət yeniləndi.", data = result.Data });
  }

  [HttpDelete("grades/{gradeId:int}")]
  public async Task<IActionResult> DeleteGrade(int gradeId, CancellationToken ct)
  {
    var result = await _grades.DeleteAsync(gradeId, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Qiymət silindi." });
  }
}
