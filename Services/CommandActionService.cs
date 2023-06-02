using AutoMapper;
using Contracts.Dto;
using Contracts.Entities;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Base;
using Services.Interfaces;

namespace Services;

public class CommandActionService : BaseService<CommandActionService>, ICommandActionService
{
    public CommandActionService(IMapper mapper, IUnitOfWorkFactory unitOfWorkFactory,
        ILogger<CommandActionService> logger) : base(mapper, unitOfWorkFactory, logger)
    {
    }

    public Task<List<CommandAction>> GetCommandActionsByTelegramBotIdAndActionTypeAsync(Guid telegramBotId,
        CommandActionType commandActionType)
    {
        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        return unitOfWork.CommandActionRepository.GetCommandActionListAsync(a =>
            a.TelegramBotId == telegramBotId && a.CommandActionType == commandActionType);
    }

    public async Task<List<CommandActionOptionDto>> GetCommandActionsByTelegramBotIdAsync(Guid telegramBotId)
    {
        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        var result = await unitOfWork.CommandActionRepository
            .GetCommandActionListAsync(a => a.TelegramBotId == telegramBotId).ConfigureAwait(false);

        return Mapper.Map<IEnumerable<CommandActionOptionDto>>(result).OrderBy(a => (int)a.CommandActionType).ToList();
    }

    public async Task CreateCommandActionAsync(CommandActionDto commandAction)
    {
        var commandActionEntity = Mapper.Map<CommandAction>(commandAction);

        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.CommandActionRepository.AddAsync(commandActionEntity).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateCommandActionAsync(Guid id, CommandActionDto commandAction)
    {
        var commandActionEntity =
            Mapper.Map<CommandAction>(commandAction, opt => { opt.AfterMap((_, dest) => { dest.Id = id; }); });

        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.CommandActionRepository.UpdateAsync(commandActionEntity).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteCommandActionAsync(Guid id)
    {
        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.CommandActionRepository.DeleteAsync(id).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}