using Contracts.Entities;

namespace Contracts.Dto;

public class CommandActionOptionDto
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public CommandActionType CommandActionType { get; set; }
}