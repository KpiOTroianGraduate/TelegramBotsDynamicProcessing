using Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using UI.Controllers.Base;

namespace UI.Controllers;

public class UserController : BaseController<UserController>
{
    private readonly IUserService _userService;

    public UserController(IUserService userService, ILogger<UserController> logger) : base(logger)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid:required}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id).ConfigureAwait(false);
            return Ok(user);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting user by id");
            return BadRequest();
        }
    }
}