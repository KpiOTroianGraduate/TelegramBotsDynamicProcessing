using Contracts.Dto;
using Contracts.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using UI.Controllers.Base;

namespace UI.Controllers;

public class CommandActionController : BaseController<CommandActionController>
{
    private readonly ICommandActionService _commandActionService;

    public CommandActionController(ICommandActionService commandActionService, ILogger<CommandActionController> logger)
        : base(logger)
    {
        _commandActionService = commandActionService;
    }

    [HttpGet("{telegramBotId:guid:required}/{commandActionType:required}/byTelegramBotId")]
    public async Task<IActionResult> GetCommandActionsByTelegramBotIdAndActionTypeAsync(Guid telegramBotId,
        CommandActionType commandActionType)
    {
        var result = await _commandActionService
            .GetCommandActionsByTelegramBotIdAndActionTypeAsync(telegramBotId, commandActionType)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{telegramBotId:guid:required}/byTelegramBotId")]
    public async Task<IActionResult> GetCommandActionsByTelegramBotIdAsync(Guid telegramBotId)
    {
        var result = await _commandActionService.GetCommandActionsByTelegramBotIdAsync(telegramBotId)
            .ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommandActionAsync([FromBody] CommandActionDto commandAction)
    {
        await _commandActionService.CreateCommandActionAsync(commandAction).ConfigureAwait(false);
        return Ok();
    }

    [HttpPut("{id:guid:required}")]
    public async Task<IActionResult> UpdateCommandActionAsync([FromRoute] Guid id,
        [FromBody] CommandActionDto commandAction)
    {
        await _commandActionService.UpdateCommandActionAsync(id, commandAction).ConfigureAwait(false);
        return Ok();
    }

    [HttpDelete("{id:guid:required}")]
    public async Task<IActionResult> DeleteCommandActionAsync([FromRoute] Guid id)
    {
        await _commandActionService.DeleteCommandActionAsync(id).ConfigureAwait(false);
        return Ok();
    }
}