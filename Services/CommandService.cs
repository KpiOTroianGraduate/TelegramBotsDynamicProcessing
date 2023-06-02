using AutoMapper;
using Contracts.Dto.Command;
using Contracts.Entities;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Base;
using Services.Interfaces;

namespace Services;

public class CommandService : BaseService<CommandService>, ICommandService
{
    public CommandService(IMapper mapper, IUnitOfWorkFactory unitOfWorkFactory, ILogger<CommandService> logger) : base(
        mapper, unitOfWorkFactory, logger)
    {
    }

    public async Task<List<CommandReadDto>> GetCommandsByTelegramBotIdAsync(Guid telegramBotId)
    {
        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        var result = await unitOfWork.CommandRepository.GetCommandsListAsync(c => c.TelegramBotId == telegramBotId)
            .ConfigureAwait(false);

        return Mapper.Map<List<CommandReadDto>>(result);
    }

    public async Task CreateCommandAsync(CommandDto command)
    {
        var commandEntity = Mapper.Map<Command>(command);

        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.CommandRepository.AddAsync(commandEntity).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateCommandAsync(Guid id, CommandDto command)
    {
        var commandEntity = Mapper.Map<Command>(command, opt => { opt.AfterMap((_, dest) => dest.Id = id); });

        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.CommandRepository.UpdateAsync(commandEntity).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteCommandAsync(Guid id)
    {
        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.CommandRepository.DeleteAsync(id).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}