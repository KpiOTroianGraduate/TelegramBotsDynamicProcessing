using Telegram.Bot.Types;

namespace Services.Interfaces;

public interface ITelegramService
{
    public Task ProcessMessageAsync(string botId, Update update);
}