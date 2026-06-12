using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Groups.Queries.GetGroupById;

public class GetGroupByIdHandler : IRequestHandler<GetGroupByIdQuery, Result<GroupDetailResponse>>
{
  private readonly IGroupRepository _groups;

  public GetGroupByIdHandler(IGroupRepository groups) => _groups = groups;

  public async Task<Result<GroupDetailResponse>> Handle(
    GetGroupByIdQuery request,
    CancellationToken ct
  )
  {
    var group =
      await _groups.GetWithDetailsAsync(request.Id, ct)
      ?? throw new NotFoundException("Group", request.Id);

    var students = group
      .GroupStudents.Where(gs => gs.Status == UserStatus.Active)
      .Select(gs => new StudentInGroup(
        gs.Student.Id,
        gs.Student.User.Name,
        gs.Student.User.Surname,
        gs.Student.Status.ToString().ToLower()
      ))
      .ToList();

    return Result<GroupDetailResponse>.Success(
      new GroupDetailResponse(
        group.Id,
        group.Name,
        new CourseInfo(group.Course.Id, group.Course.Name),
        new TeacherInfo(group.Teacher.Id, group.Teacher.User.Name, group.Teacher.User.Surname),
        group.StartDate,
        group.EndDate,
        group.Status.ToString().ToLower(),
        students,
        students.Count
      )
    );
  }
}
