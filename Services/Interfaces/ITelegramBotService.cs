using System.Security.Claims;
using Contracts.Dto.TelegramBot;
using Contracts.Entities;

namespace Services.Interfaces;

public interface ITelegramBotService
{
    public Task CreateTelegramBotAsync(TelegramBotDto telegramBot);

    public Task<List<TelegramBot>> GetTelegramBotsAsync(IEnumerable<Claim> claims);
}