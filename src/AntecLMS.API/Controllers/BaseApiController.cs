using AntecLMS.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace AntecLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
  protected IActionResult ToResponse<T>(Result<T> result)
  {
    if (!result.IsSuccess)
    {
      return result.StatusCode switch
      {
        401 => Unauthorized(new { message = result.Error }),
        403 => Forbid(),
        404 => NotFound(new { message = result.Error }),
        422 => UnprocessableEntity(new { message = result.Error, errors = result.Errors }),
        _ => BadRequest(new { message = result.Error }),
      };
    }

    return result.StatusCode == 201 ? StatusCode(201, result.Data) : Ok(result.Data);
  }

  protected IActionResult ToResponse(Result result)
  {
    if (!result.IsSuccess)
    {
      return result.StatusCode switch
      {
        404 => NotFound(new { message = result.Error }),
        _ => BadRequest(new { message = result.Error }),
      };
    }

    return Ok(new { message = "Uğurla tamamlandı." });
  }
}
