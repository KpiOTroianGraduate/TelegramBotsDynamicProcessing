using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using UI.Controllers.Base;
using Utils;

namespace UI.Controllers;

public class AuthorizationController : BaseController<AuthorizationController>
{
    private readonly IUserService _userService;

    public AuthorizationController(IUserService userService, ILogger<AuthorizationController> logger) : base(logger)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet("/login")]
    public IActionResult LogIn()
    {
        return RedirectPermanent(
            "https://login.microsoftonline.com/692688bc-3689-4b5b-885b-3f6bb78038dc/oauth2/v2.0/authorize?client_id=902dfa66-4d56-41af-bf56-347114aff037&response_type=token&scope=api://902dfa66-4d56-41af-bf56-347114aff037/users.api.scope&redirect_uri=https://localhost:8443/index.html");
    }

    [HttpGet("/registration")]
    public async Task<IActionResult> Registration()
    {
        try
        {
            var result = await _userService.RegisterUserAsync(User.Claims).ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while registration");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }
}