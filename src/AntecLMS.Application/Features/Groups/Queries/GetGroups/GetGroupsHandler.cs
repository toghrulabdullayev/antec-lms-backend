using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Groups.Queries.GetGroups;

public class GetGroupsHandler : IRequestHandler<GetGroupsQuery, Result<PagedResult<GroupListItem>>>
{
  private readonly IGroupRepository _groups;

  public GetGroupsHandler(IGroupRepository groups) => _groups = groups;

  public async Task<Result<PagedResult<GroupListItem>>> Handle(
    GetGroupsQuery request,
    CancellationToken ct
  )
  {
    GroupStatus? status = request.Status is null
      ? null
      : Enum.Parse<GroupStatus>(request.Status, true);

    var (items, total) = await _groups.GetPagedAsync(
      request.CourseId,
      request.TeacherId,
      status,
      request.Page,
      request.PerPage,
      ct
    );

    var data = items
      .Select(g => new GroupListItem(
        g.Id,
        g.Name,
        new CourseRef(g.Course.Id, g.Course.Name),
        new TeacherRef(g.Teacher.Id, g.Teacher.User.Name, g.Teacher.User.Surname),
        g.GroupStudents.Count(gs => gs.Status == UserStatus.Active),
        g.StartDate,
        g.EndDate,
        g.Status.ToString().ToLower()
      ))
      .ToList();

    return Result<PagedResult<GroupListItem>>.Success(
      new PagedResult<GroupListItem>
      {
        Data = data,
        Total = total,
        Page = request.Page,
        PerPage = request.PerPage,
      }
    );
  }
}
