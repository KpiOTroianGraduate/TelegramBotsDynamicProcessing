using Contracts.Dto.Command;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using UI.Controllers.Base;

namespace UI.Controllers;

public class CommandController : BaseController<CommandController>
{
    private readonly ICommandService _commandService;

    public CommandController(ICommandService commandService, ILogger<CommandController> logger) : base(logger)
    {
        _commandService = commandService;
    }

    [HttpGet("{telegramBotId:guid:required}/byTelegramBotId")]
    public async Task<IActionResult> GetCommandsByTelegramBotIdAsync([FromRoute] Guid telegramBotId)
    {
        var result = await _commandService.GetCommandsByTelegramBotIdAsync(telegramBotId)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommandAsync([FromBody] CommandDto command)
    {
        await _commandService.CreateCommandAsync(command).ConfigureAwait(false);
        return Ok();
    }

    [HttpPut("{id:guid:required}")]
    public async Task<IActionResult> UpdateCommandAsync([FromRoute] Guid id, [FromBody] CommandDto command)
    {
        await _commandService.UpdateCommandAsync(id, command).ConfigureAwait(false);
        return Ok();
    }

    [HttpDelete("{id:guid:required}")]
    public async Task<IActionResult> DeleteCommandAsync([FromRoute] Guid id)
    {
        await _commandService.DeleteCommandAsync(id).ConfigureAwait(false);
        return Ok();
    }
}