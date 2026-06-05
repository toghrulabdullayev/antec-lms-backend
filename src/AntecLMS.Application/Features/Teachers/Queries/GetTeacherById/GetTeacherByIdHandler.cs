using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Teachers.Queries.GetTeacherById;

public class GetTeacherByIdHandler
  : IRequestHandler<GetTeacherByIdQuery, Result<TeacherDetailResponse>>
{
  private readonly ITeacherRepository _teachers;

  public GetTeacherByIdHandler(ITeacherRepository teachers) => _teachers = teachers;

  public async Task<Result<TeacherDetailResponse>> Handle(
    GetTeacherByIdQuery request,
    CancellationToken ct
  )
  {
    var teacher =
      await _teachers.GetWithUserAsync(request.Id, ct)
      ?? throw new NotFoundException("Teacher", request.Id);

    var groups = teacher.Groups.Select(g => new GroupRef(g.Id, g.Name)).ToList();

    return Result<TeacherDetailResponse>.Success(
      new TeacherDetailResponse(
        teacher.Id,
        teacher.UserId,
        teacher.User.Name,
        teacher.User.Surname,
        teacher.User.Email,
        teacher.User.Phone,
        teacher.Specialization,
        teacher.Bio,
        teacher.Status.ToString().ToLower(),
        groups
      )
    );
  }
}
