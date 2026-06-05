using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result>
{
  private readonly IUserRepository _users;
  private readonly IGroupRepository _groups;
  private readonly IUnitOfWork _uow;

  public DeleteUserHandler(IUserRepository users, IGroupRepository groups, IUnitOfWork uow)
  {
    _users = users;
    _groups = groups;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteUserCommand request, CancellationToken ct)
  {
    var user =
      await _users.GetByIdAsync(request.Id, ct) ?? throw new NotFoundException("User", request.Id);

    if (user.TeacherProfile is not null)
    {
      var hasGroups = await _groups.HasActiveGroupsForTeacherAsync(user.TeacherProfile.Id, ct);
      if (hasGroups)
        return Result.Failure("Bu istifadəçinin aktiv qrupları var, silinə bilməz.", 400);
    }

    _users.Remove(user);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}
