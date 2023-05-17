namespace Contracts.Entities;

public class TelegramBot
{
    public Guid Id { get; set; }

    public string? Token { get; set; }

    public bool IsActive { get; set; }


    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public ICollection<Command>? Commands { get; set; }
    public ICollection<CommandAction>? CommandActions { get; set; }
}