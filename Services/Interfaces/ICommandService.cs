using Contracts.Dto.Command;

namespace Services.Interfaces;

public interface ICommandService
{
    Task<List<CommandReadDto>> GetCommandsByTelegramBotIdAsync(Guid telegramBotId);
    Task CreateCommandAsync(CommandDto command);
    Task UpdateCommandAsync(Guid id, CommandDto command);
    Task DeleteCommandAsync(Guid id);
}