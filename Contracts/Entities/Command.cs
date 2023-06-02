using Telegram.Bot.Types;

namespace Contracts.Entities;

public class Command : BotCommand
{
    public Guid Id { get; set; }

    public bool IsActive { get; set; }

    public Guid TelegramBotId { get; set; }
    public TelegramBot? TelegramBot { get; set; }

    public Guid? CommandActionId { get; set; }
    public CommandAction? CommandAction { get; set; }
}