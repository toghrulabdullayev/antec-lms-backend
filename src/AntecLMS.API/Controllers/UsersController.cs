using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : BaseApiController
{
  private readonly IUserService _users;

  public UsersController(IUserService users)
  {
    _users = users;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll(
    [FromQuery] string? role,
    [FromQuery] string? status,
    [FromQuery] string? search,
    [FromQuery] int page = 1,
    [FromQuery] int perPage = 20,
    CancellationToken ct = default
  )
  {
    var result = await _users.GetAllAsync(role, status, search, page, perPage, ct);
    return ToResponse(result);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await _users.GetByIdAsync(id, ct);
    return ToResponse(result);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken ct)
  {
    var result = await _users.CreateAsync(dto, ct);
    return ToResponse(result);
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateUserDto dto,
    CancellationToken ct
  )
  {
    var result = await _users.UpdateAsync(id, dto, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "İstifadəçi uğurla yeniləndi.", data = result.Data });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await _users.DeleteAsync(id, ct);
    if (!result.IsSuccess)
      return ToResponse(result);
    return Ok(new { message = "İstifadəçi uğurla silindi." });
  }
}
