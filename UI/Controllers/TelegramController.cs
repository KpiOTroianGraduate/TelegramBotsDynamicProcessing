using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using UI.Controllers.Base;

namespace UI.Controllers;

[AllowAnonymous]
public class TelegramController : BaseController<TelegramController>
{
    private readonly ITelegramService _telegramService;

    public TelegramController(ITelegramService telegramService, ILogger<TelegramController> logger) : base(logger)
    {
        _telegramService = telegramService;
    }

    [HttpGet("{botId}")]
    public async Task<IActionResult> SetUpBot([FromRoute] string botId)
    {
        try
        {
            await _telegramService.SetUpBotAsync(botId).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"SetUpBot, botId: {botId}, message: {ex.Message}");
            return BadRequest();
        }
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