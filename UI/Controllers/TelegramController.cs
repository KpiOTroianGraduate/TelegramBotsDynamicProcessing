using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace UI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TelegramController : ControllerBase
{
    private readonly ILogger<TelegramController> _logger;
    private readonly ITelegramService _telegramService;

    public TelegramController(ITelegramService telegramService, ILogger<TelegramController> logger)
    {
        _telegramService = telegramService;
        _logger = logger;
    }

    [HttpPost("{botId}")]
    public async Task<IActionResult> GetUpdateFromTelegram([FromRoute] string botId, [FromBody] Update update)
    {
        try
        {
            await _telegramService.ProcessMessageAsync(botId, update);

            return Ok();
        }
        catch (ApiRequestException ex)
        {
            _logger.LogError(ex, $"GetMessageFromTelegram, botId: {botId}, update: {update}, message: {ex.Message}");
            return BadRequest("Bot is unavailable");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"GetMessageFromTelegram, botId: {botId}, update: {update}, message: {ex.Message}");
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _telegramService.ProcessMessageAsync("", new Update()).ConfigureAwait(false);
        return Ok();
    }
}