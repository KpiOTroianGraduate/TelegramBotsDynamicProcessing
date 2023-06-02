using System.Linq.Expressions;
using Contracts.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories;

public class CommandRepository : BaseRepository<TelegramContext, CommandRepository>, ICommandRepository
{
    public CommandRepository(TelegramContext db, ILogger<CommandRepository> logger) : base(db, logger)
    {
    }

    public async Task AddAsync(Command item)
    {
        await Db.Commands.AddAsync(item).ConfigureAwait(false);
    }

    public Task<Command> GetAsync(Guid id)
    {
        return Db.Commands.FirstAsync(c => c.Id == id);
    }

    public Task<Command?> GetFirstOrDefaultAsync(Expression<Func<Command, bool>> where)
    {
        return Db.Commands.FirstOrDefaultAsync(where);
    }

    public Task<List<Command>> GetAllAsync()
    {
        return Db.Commands.ToListAsync();
    }

    public async Task UpdateAsync(Command item)
    {
        var command = await Db.Commands.FirstAsync(c => c.Id == item.Id).ConfigureAwait(false);
        Db.Commands.Entry(command).CurrentValues.SetValues(item);
    }

    public async Task DeleteAsync(Guid id)
    {
        var command = await Db.Commands.FirstAsync(c => c.Id == id).ConfigureAwait(false);
        Db.Commands.Remove(command);
    }

    public Task<List<Command>> GetCommandsListAsync(Expression<Func<Command, bool>>? where = null)
    {
        return where != null ? Db.Commands.Where(where).ToListAsync() : Db.Commands.ToListAsync();
    }
}