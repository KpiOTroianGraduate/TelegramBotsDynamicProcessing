using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers.Base;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public abstract class BaseController<T> : ControllerBase where T : class
{
    protected readonly ILogger<T> Logger;

    protected BaseController(ILogger<T> logger)
    {
        Logger = logger;
    }
}