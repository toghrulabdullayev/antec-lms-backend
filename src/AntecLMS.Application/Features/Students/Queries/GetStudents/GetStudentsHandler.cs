using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Students.Queries.GetStudents;

public class GetStudentsHandler
  : IRequestHandler<GetStudentsQuery, Result<PagedResult<StudentListItem>>>
{
  private readonly IStudentRepository _students;

  public GetStudentsHandler(IStudentRepository students) => _students = students;

  public async Task<Result<PagedResult<StudentListItem>>> Handle(
    GetStudentsQuery request,
    CancellationToken ct
  )
  {
    UserStatus? status = request.Status is null
      ? null
      : Enum.Parse<UserStatus>(request.Status, true);

    var (items, total) = await _students.GetPagedAsync(
      request.GroupId,
      status,
      request.Search,
      request.Page,
      request.PerPage,
      ct
    );

    var data = items
      .Select(s => new StudentListItem(
        s.Id,
        s.UserId,
        s.User.Name,
        s.User.Surname,
        s.User.Email,
        s.User.Phone,
        s.BirthDate,
        s.Status.ToString().ToLower(),
        s.GroupStudents.Where(gs => gs.Status == UserStatus.Active)
          .Select(gs => new GroupRefS(gs.Group.Id, gs.Group.Name))
          .ToList()
      ))
      .ToList();

    return Result<PagedResult<StudentListItem>>.Success(
      new PagedResult<StudentListItem>
      {
        Data = data,
        Total = total,
        Page = request.Page,
        PerPage = request.PerPage,
      }
    );
  }
}
