using System.Linq.Expressions;
using Contracts.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories;

public sealed class CommandActionRepository : BaseRepository<ITelegramContext, CommandActionRepository>,
    ICommandActionRepository
{
    public CommandActionRepository(ITelegramContext db, ILogger<CommandActionRepository> logger) : base(db, logger)
    {
    }

    public async Task AddAsync(CommandAction item)
    {
        await Db.CommandActions.AddAsync(item).ConfigureAwait(false);
    }

    public Task<CommandAction> GetAsync(Guid id)
    {
        return Db.CommandActions.FirstAsync(a => a.Id == id);
    }

    public Task<CommandAction?> GetFirstOrDefaultAsync(Expression<Func<CommandAction, bool>> where)
    {
        return Db.CommandActions.FirstOrDefaultAsync(where);
    }

    public Task<List<CommandAction>> GetAllAsync()
    {
        return Db.CommandActions.ToListAsync();
    }

    public async Task UpdateAsync(CommandAction item)
    {
        var commandAction = await Db.CommandActions.FirstAsync(a => a.Id == item.Id).ConfigureAwait(false);
        Db.CommandActions.Entry(commandAction).CurrentValues.SetValues(item);
    }

    public async Task DeleteAsync(Guid id)
    {
        var commandAction = await Db.CommandActions.FirstAsync(a => a.Id == id).ConfigureAwait(false);
        Db.CommandActions.Remove(commandAction);
    }

    public Task<List<CommandAction>> GetCommandActionListAsync(Expression<Func<CommandAction, bool>>? where = null)
    {
        return where != null ? Db.CommandActions.Where(where).ToListAsync() : Db.CommandActions.ToListAsync();
    }
}