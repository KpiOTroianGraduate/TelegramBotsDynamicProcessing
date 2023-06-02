﻿namespace Contracts.Entities;

public class User
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public ICollection<TelegramBot>? TelegramBots { get; set; }
}