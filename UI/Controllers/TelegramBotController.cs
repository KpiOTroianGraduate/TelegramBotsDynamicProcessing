﻿using Contracts.Dto.TelegramBot;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using UI.Controllers.Base;
using Utils;

namespace UI.Controllers;

public class TelegramBotController : BaseController<TelegramBotController>
{
    private readonly ITelegramBotService _telegramBotService;
    private readonly ITelegramService _telegramService;

    public TelegramBotController(ITelegramBotService telegramBotService, ITelegramService telegramService,
        ILogger<TelegramBotController> logger) :
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
        try
        {

            await _telegramBotService.CreateTelegramBotAsync(User.Claims, telegramBot).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while updating telegram bot");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpPut("{id:guid:required}")]
    public async Task<IActionResult> UpdateTelegramBotAsync([FromRoute] Guid id, [FromBody] TelegramBotDto telegramBot)
    {
        try
        {
            await _telegramBotService.UpdateTelegramBotAsync(id, telegramBot).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while updating telegram bot");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }

    [HttpDelete("{id:guid:required}")]
    public async Task<IActionResult> DeleteTelegramBotAsync([FromRoute] Guid id)
    {
        try
        {
            await _telegramBotService.DeleteTelegramBotAsync(id).ConfigureAwait(false);
            return Ok();
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Error while deleting telegram bot");
            return BadRequest(ex.GetErrorMessageJson());
        }
    }
}