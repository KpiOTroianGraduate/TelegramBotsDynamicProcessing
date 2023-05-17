using Telegram.Bot.Types;

namespace Contracts.Models;

public class TelegramMessage
{
    public string BotId { get; set; } = null!;

    public Update Update { get; set; } = null!;
}