using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Groups.Commands.RemoveStudentFromGroup;

public class RemoveStudentFromGroupHandler : IRequestHandler<RemoveStudentFromGroupCommand, Result>
{
  private readonly IGroupRepository _groups;
  private readonly IUnitOfWork _uow;

  public RemoveStudentFromGroupHandler(IGroupRepository groups, IUnitOfWork uow)
  {
    _groups = groups;
    _uow = uow;
  }

  public async Task<Result> Handle(RemoveStudentFromGroupCommand request, CancellationToken ct)
  {
    var gs = await _groups.GetGroupStudentAsync(request.GroupId, request.StudentId, ct);
    if (gs is null)
      return Result.Failure("Tələbə bu qrupda deyil.", 404);

    gs.Status = UserStatus.Inactive;
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}
