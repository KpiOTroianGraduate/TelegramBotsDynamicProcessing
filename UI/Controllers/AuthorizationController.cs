using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Controllers.Base;

namespace UI.Controllers;

[AllowAnonymous]
public class AuthorizationController : BaseController<AuthorizationController>
{
    public AuthorizationController(ILogger<AuthorizationController> logger) : base(logger)
    {
    }

    [HttpGet("/login")]
    public IActionResult LogIn()
    {
        return RedirectPermanent(
            "https://login.microsoftonline.com/692688bc-3689-4b5b-885b-3f6bb78038dc/oauth2/v2.0/authorize?client_id=902dfa66-4d56-41af-bf56-347114aff037&response_type=token&scope=api://902dfa66-4d56-41af-bf56-347114aff037/users.api.scope&redirect_uri=https://hapan9-telegram.azurewebsites.net/index.html");
    }

    [HttpGet]
    public IActionResult Receive()
    {
        var a = User.Identities.First(c => c.NameClaimType == ClaimTypes.Name);
        var b = User.Identity;
        return Ok();
    }
}