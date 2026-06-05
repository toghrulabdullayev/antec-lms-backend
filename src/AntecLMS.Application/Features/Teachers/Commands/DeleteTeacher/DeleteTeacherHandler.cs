using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Teachers.Commands.DeleteTeacher;

public class DeleteTeacherHandler : IRequestHandler<DeleteTeacherCommand, Result>
{
  private readonly ITeacherRepository _teachers;
  private readonly IGroupRepository _groups;
  private readonly IUnitOfWork _uow;

  public DeleteTeacherHandler(ITeacherRepository teachers, IGroupRepository groups, IUnitOfWork uow)
  {
    _teachers = teachers;
    _groups = groups;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteTeacherCommand request, CancellationToken ct)
  {
    var teacher =
      await _teachers.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Teacher", request.Id);

    if (await _groups.HasActiveGroupsForTeacherAsync(request.Id, ct))
      return Result.Failure("Bu müəllimin aktiv qrupları var, silinə bilməz.", 400);

    _teachers.Remove(teacher);
    await _uow.SaveChangesAsync(ct);

    return Result.Success();
  }
}
