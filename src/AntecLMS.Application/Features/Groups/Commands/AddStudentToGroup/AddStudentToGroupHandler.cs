using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Entities;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Groups.Commands.AddStudentToGroup;

public class AddStudentToGroupHandler
  : IRequestHandler<AddStudentToGroupCommand, Result<AddStudentResponse>>
{
  private readonly IGroupRepository _groups;
  private readonly IStudentRepository _students;
  private readonly IUnitOfWork _uow;

  public AddStudentToGroupHandler(
    IGroupRepository groups,
    IStudentRepository students,
    IUnitOfWork uow
  )
  {
    _groups = groups;
    _students = students;
    _uow = uow;
  }

  public async Task<Result<AddStudentResponse>> Handle(
    AddStudentToGroupCommand request,
    CancellationToken ct
  )
  {
    var group = await _groups.GetByIdAsync(request.GroupId, ct);
    if (group is null)
      return Result<AddStudentResponse>.Failure("Qrup tapılmadı.", 404);

    var student = await _students.GetByIdAsync(request.StudentId, ct);
    if (student is null)
      return Result<AddStudentResponse>.Failure("Tələbə tapılmadı.", 404);

    if (await _groups.StudentExistsInGroupAsync(request.GroupId, request.StudentId, ct))
      return Result<AddStudentResponse>.Failure("Bu tələbə artıq bu qrupdadır.", 400);

    var gs = new GroupStudent
    {
      GroupId = request.GroupId,
      StudentId = request.StudentId,
      JoinedAt = DateTime.UtcNow,
      Status = UserStatus.Active,
    };

    await _groups.AddStudentAsync(gs, ct);
    await _uow.SaveChangesAsync(ct);

    return Result<AddStudentResponse>.Success(
      new AddStudentResponse(gs.GroupId, gs.StudentId, gs.JoinedAt, "active"),
      201
    );
  }
}
