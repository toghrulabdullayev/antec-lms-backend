using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Enums;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Students.Queries.GetStudentById;

public class GetStudentByIdHandler
  : IRequestHandler<GetStudentByIdQuery, Result<StudentDetailResponse>>
{
  private readonly IStudentRepository _students;

  public GetStudentByIdHandler(IStudentRepository students) => _students = students;

  public async Task<Result<StudentDetailResponse>> Handle(
    GetStudentByIdQuery request,
    CancellationToken ct
  )
  {
    var student =
      await _students.GetWithUserAsync(request.Id, ct)
      ?? throw new NotFoundException("Student", request.Id);

    var groups = student
      .GroupStudents.Where(gs => gs.Status == UserStatus.Active)
      .Select(gs => new GroupRefD(gs.Group.Id, gs.Group.Name))
      .ToList();

    return Result<StudentDetailResponse>.Success(
      new StudentDetailResponse(
        student.Id,
        student.UserId,
        student.User.Name,
        student.User.Surname,
        student.User.Email,
        student.User.Phone,
        student.BirthDate,
        student.Note,
        student.Status.ToString().ToLower(),
        groups
      )
    );
  }
}
