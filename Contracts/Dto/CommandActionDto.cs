using Contracts.Entities;

namespace Contracts.Dto;

public class CommandActionDto
{
    public string? Content { get; set; }

    public CommandActionType CommandActionType { get; set; }

    public Guid TelegramBotId { get; set; }
}