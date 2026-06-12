using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateTeacherDto(
  string Name,
  string Surname,
  string Email,
  string Password,
  string? Phone,
  string? Specialization,
  string? Bio,
  string Status
);

public record UpdateTeacherDto(string? Phone, string? Specialization, string Status);

public record TeacherResponse(
  int Id,
  int UserId,
  string Name,
  string Surname,
  string Email,
  string? Specialization,
  string Status
);

public record TeacherListItem(
  int Id,
  int UserId,
  string Name,
  string Surname,
  string Email,
  string? Phone,
  string? Specialization,
  string Status
);

public record TeacherDetailResponse(
  int Id,
  int UserId,
  string Name,
  string Surname,
  string Email,
  string? Phone,
  string? Specialization,
  string? Bio,
  string Status,
  List<GroupRefT> Groups
);

public record GroupRefT(int Id, string Name);
