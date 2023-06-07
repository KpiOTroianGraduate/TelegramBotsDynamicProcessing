namespace Contracts.Dto.TelegramBot;

public class TelegramBotDto
{
    public string Token { get; set; } = null!;

    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
}