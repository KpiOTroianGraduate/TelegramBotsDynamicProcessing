using Contracts.Dto.Command;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using UI.Controllers.Base;
using Utils;

namespace UI.Controllers;

public sealed class CommandController : BaseController<CommandController>
{
    private readonly ICommandService _commandService;
    private readonly IVerifyService _verifyService;

    public CommandController(ICommandService commandService, IVerifyService verifyService,
        ILogger<CommandController> logger) : base(logger)
    {
        _commandService = commandService;
        _verifyService = verifyService;
    }

    [HttpGet("{telegramBotId:guid:required}/byTelegramBotId")]
    public async Task<IActionResult> GetCommandsByTelegramBotIdAsync([FromRoute] Guid telegramBotId)
    {
        try
        {
            if (!await _verifyService.VerifyTelegramBotAsync(User.Claims, telegramBotId).ConfigureAwait(false))
                return NotFound();
            var result = await _commandService.GetCommandsByTelegramBotIdAsync(telegramBotId)
                .ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while getting commands by telegram bot id");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommandAsync([FromBody] CommandDto command)
    {
        try
        {
            if (!await _verifyService.VerifyTelegramBotAsync(User.Claims, command.TelegramBotId).ConfigureAwait(false))
                return NotFound();
            if (command.CommandActionId != null)
                if (!await _verifyService.VerifyCommandActionAsync(User.Claims, command.CommandActionId.Value)
                        .ConfigureAwait(false))
                    return NotFound();

            await _commandService.CreateCommandAsync(command).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while getting commands by telegram bot id");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpPut("{id:guid:required}")]
    public async Task<IActionResult> UpdateCommandAsync([FromRoute] Guid id, [FromBody] CommandDto command)
    {
        try
        {
            if (!await _verifyService.VerifyCommandAsync(User.Claims, id).ConfigureAwait(false)) return NotFound();
            if (command.CommandActionId != null)
                if (!await _verifyService.VerifyCommandActionAsync(User.Claims, command.CommandActionId.Value)
                        .ConfigureAwait(false))
                    return NotFound();

            await _commandService.UpdateCommandAsync(id, command).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while getting commands by telegram bot id");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpDelete("{id:guid:required}")]
    public async Task<IActionResult> DeleteCommandAsync([FromRoute] Guid id)
    {
        try
        {
            if (!await _verifyService.VerifyCommandAsync(User.Claims, id).ConfigureAwait(false)) return NotFound();

            await _commandService.DeleteCommandAsync(id).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while getting commands by telegram bot id");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }
}