namespace Contracts.Entities;

public class CommandAction
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public CommandActionType CommandActionType { get; set; }

    public Guid TelegramBotId { get; set; }
    public TelegramBot? TelegramBot { get; set; }

    public ICollection<Command>? Commands { get; set; }
}

public enum CommandActionType
{
    Text,
    HttpPost,
    InlineKeyboard,
    ReplyKeyboard
}