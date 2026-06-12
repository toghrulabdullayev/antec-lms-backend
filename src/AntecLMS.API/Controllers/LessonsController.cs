using AntecLMS.Application.Features.Attendances.Commands.CreateAttendance;
using AntecLMS.Application.Features.Attendances.Commands.DeleteAttendance;
using AntecLMS.Application.Features.Attendances.Commands.UpdateAttendance;
using AntecLMS.Application.Features.Attendances.Queries.GetLessonAttendances;
using AntecLMS.Application.Features.Grades.Commands.CreateGrade;
using AntecLMS.Application.Features.Grades.Commands.DeleteGrade;
using AntecLMS.Application.Features.Grades.Commands.UpdateGrade;
using AntecLMS.Application.Features.Grades.Queries.GetLessonGrades;
using AntecLMS.Application.Features.Lessons.Commands.CreateLesson;
using AntecLMS.Application.Features.Lessons.Commands.DeleteLesson;
using AntecLMS.Application.Features.Lessons.Commands.UpdateLesson;
using AntecLMS.Application.Features.Lessons.Queries.GetGroupLessons;
using AntecLMS.Application.Features.Lessons.Queries.GetLessonById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin,Teacher")]
public class LessonsController : BaseApiController
{
  [HttpGet("group/{groupId:int}")]
  public async Task<IActionResult> GetByGroup(int groupId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetGroupLessonsQuery(groupId), ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetLessonByIdQuery(id), ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create(
    [FromBody] CreateLessonCommand command,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(command, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Dərs uğurla yaradıldı.", data = result.Data });
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateLessonRequest request,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(
      new UpdateLessonCommand(id, request.LessonDate, request.Topic, request.Note, request.Status),
      ct
    );
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Dərs uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await Mediator.Send(new DeleteLessonCommand(id), ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Dərs uğurla silindi." });
  }

  // --- Attendances per lesson ---

  [HttpGet("{lessonId:int}/attendances")]
  public async Task<IActionResult> GetAttendances(int lessonId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetLessonAttendancesQuery(lessonId), ct);
    return ToResponse(result);
  }

  [HttpPost("{lessonId:int}/attendances")]
  public async Task<IActionResult> CreateAttendance(
    int lessonId,
    [FromBody] CreateAttendanceRequest request,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(
      new CreateAttendanceCommand(
        lessonId,
        request.StudentId,
        request.Status,
        request.MinutesLate,
        request.Reason,
        request.TeacherNote
      ),
      ct
    );
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Davamiyyət qeyd edildi.", data = result.Data });
  }

  [HttpPut("attendances/{attendanceId:int}")]
  public async Task<IActionResult> UpdateAttendance(
    int attendanceId,
    [FromBody] UpdateAttendanceRequest request,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(
      new UpdateAttendanceCommand(
        attendanceId,
        request.Status,
        request.MinutesLate,
        request.Reason,
        request.TeacherNote
      ),
      ct
    );
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Davamiyyət yeniləndi.", data = result.Data });
  }

  [HttpDelete("attendances/{attendanceId:int}")]
  public async Task<IActionResult> DeleteAttendance(int attendanceId, CancellationToken ct)
  {
    var result = await Mediator.Send(new DeleteAttendanceCommand(attendanceId), ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Davamiyyət silindi." });
  }

  // --- Grades per lesson ---

  [HttpGet("{lessonId:int}/grades")]
  public async Task<IActionResult> GetGrades(int lessonId, CancellationToken ct)
  {
    var result = await Mediator.Send(new GetLessonGradesQuery(lessonId), ct);
    return ToResponse(result);
  }

  [HttpPost("{lessonId:int}/grades")]
  public async Task<IActionResult> CreateGrade(
    int lessonId,
    [FromBody] CreateGradeRequest request,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(
      new CreateGradeCommand(
        lessonId,
        request.StudentId,
        request.Score,
        request.MaxScore,
        request.TeacherNote
      ),
      ct
    );
    if (!result.IsSuccess)
      return ToResponse(result);
    return StatusCode(201, new { message = "Qiymət qeyd edildi.", data = result.Data });
  }

  [HttpPut("grades/{gradeId:int}")]
  public async Task<IActionResult> UpdateGrade(
    int gradeId,
    [FromBody] UpdateGradeRequest request,
    CancellationToken ct
  )
  {
    var result = await Mediator.Send(
      new UpdateGradeCommand(gradeId, request.Score, request.MaxScore, request.TeacherNote),
      ct
    );
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Qiymət yeniləndi.", data = result.Data });
  }

  [HttpDelete("grades/{gradeId:int}")]
  public async Task<IActionResult> DeleteGrade(int gradeId, CancellationToken ct)
  {
    var result = await Mediator.Send(new DeleteGradeCommand(gradeId), ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "Qiymət silindi." });
  }
}

public record UpdateLessonRequest(
  DateTime? LessonDate,
  string? Topic,
  string? Note,
  string? Status
);

public record CreateAttendanceRequest(
  int StudentId,
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote
);

public record UpdateAttendanceRequest(
  string Status,
  int? MinutesLate,
  string? Reason,
  string? TeacherNote
);

public record CreateGradeRequest(int StudentId, int Score, int MaxScore, string? TeacherNote);

public record UpdateGradeRequest(int Score, int MaxScore, string? TeacherNote);
