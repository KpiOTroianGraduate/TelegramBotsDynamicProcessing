namespace Contracts.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public ICollection<TelegramBot> TelegramBots { get; set; } = new List<TelegramBot>();
}