using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Groups.Commands.DeleteGroup;

public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, Result>
{
  private readonly IGroupRepository _groups;
  private readonly IUnitOfWork _uow;

  public DeleteGroupHandler(IGroupRepository groups, IUnitOfWork uow)
  {
    _groups = groups;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteGroupCommand request, CancellationToken ct)
  {
    var group =
      await _groups.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Group", request.Id);

    _groups.Remove(group);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}
