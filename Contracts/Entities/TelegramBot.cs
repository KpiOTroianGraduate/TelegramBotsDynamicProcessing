namespace Contracts.Entities;

public class TelegramBot
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public bool IsActive { get; set; }


    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public ICollection<Command> Commands { get; set; } = null!;
    public ICollection<CommandAction> CommandActions { get; set; } = null!;
}