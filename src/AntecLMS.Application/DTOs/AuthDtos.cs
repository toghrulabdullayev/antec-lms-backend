using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record LoginDto(string Email, string Password);

public record LoginResponse(string Token, UserDto User);

public record UserDto(
  int Id,
  string Name,
  string Surname,
  string Email,
  string Role,
  string Status
);

public record MeResponse(
  int Id,
  string Name,
  string Surname,
  string Email,
  string Role,
  string? Phone,
  string Status
);
