using Contracts.Dto;
using Contracts.Entities;

namespace Services.Interfaces;

public interface ICommandActionService
{
    Task<List<CommandAction>> GetCommandActionsByTelegramBotIdAndActionTypeAsync(Guid telegramBotId,
        CommandActionType commandActionType);

    Task<List<CommandActionOptionDto>> GetCommandActionsByTelegramBotIdAsync(Guid telegramBotId);
    Task CreateCommandActionAsync(CommandActionDto commandAction);
    Task UpdateCommandActionAsync(Guid id, CommandActionDto commandAction);
    Task DeleteCommandActionAsync(Guid id);
}