namespace Contracts.Dto.Command;

public class CommandDto
{
    public bool IsActive { get; set; }

    public Guid TelegramBotId { get; set; }

    public Guid? CommandActionId { get; set; }

    public string Command { get; set; } = null!;

    public string? Description { get; set; }
}