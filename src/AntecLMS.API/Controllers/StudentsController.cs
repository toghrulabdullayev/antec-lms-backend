using AntecLMS.Application.Features.Attendances.Queries.GetStudentAttendances;
using AntecLMS.Application.Features.Grades.Queries.GetStudentGrades;
using AntecLMS.Application.Features.Students.Commands.CreateStudent;
using AntecLMS.Application.Features.Students.Commands.DeleteStudent;
using AntecLMS.Application.Features.Students.Commands.UpdateStudent;
using AntecLMS.Application.Features.Students.Queries.GetStudentById;
using AntecLMS.Application.Features.Students.Queries.GetStudents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin")]
public class StudentsController : BaseApiController
{
  [HttpGet]
  public async Task<IActionResult> GetAll(
    [FromQuery] int? groupId,
    [FromQuery] string? status,
    [FromQuery] string? search,
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 20,
    CancellationToken ct = default
  )
  {
    var result = await Mediator.Send(
      new GetStudentsQuery(groupId, status, search, page, perPage),
      ct
    );
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  [Authorize(Roles = "Admin,Teacher")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetStudentByIdQuery(id), ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create(
    [FromBody] CreateStudentCommand command,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(command, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Tələbə uğurla yaradıldı.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateStudentRequest request,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(
      new UpdateStudentCommand(id, request.Phone, request.Note, request.Status),
      ct
    );
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Tələbə məlumatları uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await Mediator.Send(new DeleteStudentCommand(id), ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Tələbə uğurla silindi." });
  }

  // --- Student attendance history (read-only, use LessonsController for CRUD) ---

  [HttpGet("{studentId:int}/attendances")]
  [Authorize(Roles = "Admin,Teacher")]
  public async Task<IActionResult> GetAttendances(int studentId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetStudentAttendancesQuery(studentId), ct);
    return ToResponse(result);
  }

  // --- Student grade history (read-only, use LessonsController for CRUD) ---

  [HttpGet("{studentId:int}/grades")]
  [Authorize(Roles = "Admin,Teacher")]
  public async Task<IActionResult> GetGrades(int studentId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetStudentGradesQuery(studentId), ct);
    return ToResponse(result);
  }
}

public record UpdateStudentRequest(string? Phone, string? Note, string Status);
