namespace Contracts.Entities;

public class User
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public ICollection<TelegramBot>? TelegramBots { get; set; }
}