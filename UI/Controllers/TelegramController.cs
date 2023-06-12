using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Telegram.Bot.Types;
using UI.Controllers.Base;

namespace UI.Controllers;

[AllowAnonymous]
public sealed class TelegramController : BaseController<TelegramController>
{
    private readonly ITelegramService _telegramService;

    public TelegramController(ITelegramService telegramService, ILogger<TelegramController> logger) : base(logger)
    {
        _telegramService = telegramService;
    }

    [HttpPost("{botId:required}")]
    public async Task<IActionResult> GetUpdateFromTelegram([FromRoute] string botId, [FromBody] Update update)
    {
        try
        {
            await _telegramService.ProcessMessageAsync(botId, update)
                .ConfigureAwait(false);

            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                $"GetMessageFromTelegram, botId: {botId}, update: {update}, message: {ex.Message}");
            return BadRequest();
        }
    }
}