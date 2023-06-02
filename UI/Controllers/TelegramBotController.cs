using Contracts.Dto.TelegramBot;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using UI.Controllers.Base;

namespace UI.Controllers;

public class TelegramBotController : BaseController<TelegramBotController>
{
    private readonly ITelegramBotService _telegramBotService;
    private readonly ITelegramService _telegramService;

    public TelegramBotController(ITelegramBotService telegramBotService, ITelegramService telegramService, ILogger<TelegramBotController> logger) :
        base(logger)
    {
        _telegramBotService = telegramBotService;
        _telegramService = telegramService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTelegramBotsAsync()
    {
        var telegramBots = await _telegramBotService.GetTelegramBotsAsync(User.Claims).ConfigureAwait(false);
        var telegramBotsInfo = await _telegramService.GetBotsInfoAsync(telegramBots).ConfigureAwait(false);
        return Ok(telegramBotsInfo);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTelegramBotAsync([FromBody] TelegramBotDto telegramBot)
    {
        await _telegramBotService.CreateTelegramBotAsync(telegramBot).ConfigureAwait(false);
        return Ok();
    }
}