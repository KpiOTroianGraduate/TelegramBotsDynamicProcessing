using Contracts.Dto.TelegramBot;
using Contracts.Entities;
using Telegram.Bot.Types;

namespace Services.Interfaces;

public interface ITelegramService
{
    public Task ProcessMessageAsync(string botId, Update update);

    Task<bool> IsBotAvailableAsync(string botId);
    Task SetWebHookAsync(string botId);
    Task DeleteWebHookAsync(string botId);
    Task<List<TelegramBotInfoDto>> GetBotsInfoAsync(List<TelegramBot> telegramBots);
    Task<TelegramBotDescriptionDto> GetDescriptionAsync(string botId);
    Task ChangeNameAsync(string botId, string? value);
    Task ChangeDescriptionAsync(string botId, string? value);
    Task ChangeShortDescriptionAsync(string botId, string? value);
    Task ChangeCommandsAsync(string botId, IEnumerable<BotCommand> commands);
}