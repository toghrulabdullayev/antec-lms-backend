using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateUserDto(
  string Name,
  string Surname,
  string Email,
  string Password,
  string? Phone,
  string Role,
  string Status
);

public record UpdateUserDto(string Name, string Surname, string? Phone, string Status);

public record UserResponse(
  int Id,
  string Name,
  string Surname,
  string Email,
  string? Phone,
  string Role,
  string Status,
  DateTime CreatedAt
);

public record UserListItem(
  int Id,
  string Name,
  string Surname,
  string Email,
  string Role,
  string? Phone,
  string Status,
  DateTime CreatedAt
);

public record UserDetailResponse(
  int Id,
  string Name,
  string Surname,
  string Email,
  string Role,
  string? Phone,
  string Status,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);

