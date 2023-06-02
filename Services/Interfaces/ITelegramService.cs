using Contracts.Dto.TelegramBot;
using Contracts.Entities;
using Telegram.Bot.Types;

namespace Services.Interfaces;

public interface ITelegramService
{
    public Task ProcessMessageAsync(string botId, Update update);

    Task SetUpBotAsync(string botId);

    Task<List<TelegramBotInfoDto>> GetBotsInfoAsync(List<TelegramBot> telegramBots);
}