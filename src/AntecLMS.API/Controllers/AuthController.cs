using AntecLMS.Application.DTOs;
using AntecLMS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

public class AuthController : BaseApiController
{
  private readonly IAuthService _auth;

  public AuthController(IAuthService auth)
  {
    _auth = auth;
  }

  [HttpPost("login")]
  [AllowAnonymous]
  public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
  {
    var result = await _auth.LoginAsync(dto, ct);

    if (!result.IsSuccess)
      return Unauthorized(new { message = result.Error });

    return Ok(new { token = result.Data!.Token, user = result.Data.User });
  }

  [HttpGet("me")]
  [Authorize]
  public async Task<IActionResult> Me(CancellationToken ct)
  {
    var result = await _auth.GetMeAsync(ct);
    return ToResponse(result);
  }

  [HttpPost("logout")]
  [Authorize]
  public async Task<IActionResult> Logout(CancellationToken ct)
  {
    var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    var result = await _auth.LogoutAsync(token, ct);
    return ToResponse(result);
  }
}
