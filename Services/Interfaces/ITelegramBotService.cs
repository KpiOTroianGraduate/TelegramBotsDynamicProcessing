using System.Security.Claims;
using Contracts.Dto.TelegramBot;
using Contracts.Entities;

namespace Services.Interfaces;

public interface ITelegramBotService
{
    public Task<List<TelegramBot>> GetTelegramBotsAsync(IEnumerable<Claim> claims);

    public Task CreateTelegramBotAsync(IEnumerable<Claim> claims, TelegramBotDto telegramBot);
    public Task UpdateTelegramBotAsync(Guid id, TelegramBotDto telegramBot);

    public Task DeleteTelegramBotAsync(Guid id);
}