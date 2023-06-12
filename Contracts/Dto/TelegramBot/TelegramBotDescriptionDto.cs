namespace Contracts.Dto.TelegramBot;

public class TelegramBotDescriptionDto
{
    public string? UserName { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }
}