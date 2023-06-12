using Contracts.Dto;
using Contracts.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using UI.Controllers.Base;
using Utils;

namespace UI.Controllers;

public sealed class CommandActionController : BaseController<CommandActionController>
{
    private readonly ICommandActionService _commandActionService;
    private readonly IVerifyService _verifyService;

    public CommandActionController(ICommandActionService commandActionService, IVerifyService verifyService,
        ILogger<CommandActionController> logger)
        : base(logger)
    {
        _commandActionService = commandActionService;
        _verifyService = verifyService;
    }

    [HttpGet("{telegramBotId:guid:required}/{commandActionType:required}/byTelegramBotId")]
    public async Task<IActionResult> GetCommandActionsByTelegramBotIdAndActionTypeAsync(Guid telegramBotId,
        CommandActionType commandActionType)
    {
        try
        {
            if (!await _verifyService.VerifyTelegramBotAsync(User.Claims, telegramBotId).ConfigureAwait(false))
                return NotFound();
            var result = await _commandActionService
                .GetCommandActionsByTelegramBotIdAndActionTypeAsync(telegramBotId, commandActionType)
                .ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while getting commands action by telegram bot id and type");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpGet("{telegramBotId:guid:required}/byTelegramBotId")]
    public async Task<IActionResult> GetCommandActionsByTelegramBotIdAsync(Guid telegramBotId)
    {
        try
        {
            if (!await _verifyService.VerifyTelegramBotAsync(User.Claims, telegramBotId).ConfigureAwait(false))
                return NotFound();

            var result = await _commandActionService.GetCommandActionsByTelegramBotIdAsync(telegramBotId)
                .ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while getting commands action by telegram bot id");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommandActionAsync([FromBody] CommandActionDto commandAction)
    {
        try
        {
            if (!await _verifyService.VerifyTelegramBotAsync(User.Claims, commandAction.TelegramBotId)
                    .ConfigureAwait(false)) return NotFound();

            await _commandActionService.CreateCommandActionAsync(commandAction).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while creating commands action");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpPut("{id:guid:required}")]
    public async Task<IActionResult> UpdateCommandActionAsync([FromRoute] Guid id,
        [FromBody] CommandActionDto commandAction)
    {
        try
        {
            if (!await _verifyService.VerifyCommandActionAsync(User.Claims, id).ConfigureAwait(false))
                return NotFound();

            await _commandActionService.UpdateCommandActionAsync(id, commandAction).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while updating commands action");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpDelete("{id:guid:required}")]
    public async Task<IActionResult> DeleteCommandActionAsync([FromRoute] Guid id)
    {
        try
        {
            if (!await _verifyService.VerifyCommandActionAsync(User.Claims, id).ConfigureAwait(false))
                return NotFound();

            await _commandActionService.DeleteCommandActionAsync(id).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while deleting commands action");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }
}