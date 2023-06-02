using System.Linq.Expressions;
using Contracts.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories;

public class TelegramBotRepository : BaseRepository<TelegramContext, TelegramBotRepository>, ITelegramBotRepository
{
    public TelegramBotRepository(TelegramContext db, ILogger<TelegramBotRepository> logger) : base(db, logger)
    {
    }


    public async Task AddAsync(TelegramBot item)
    {
        await Db.TelegramBots.AddAsync(item).ConfigureAwait(false);
    }

    public Task<TelegramBot> GetAsync(Guid id)
    {
        return Db.TelegramBots.FirstAsync(b => b.Id == id);
    }

    public Task<TelegramBot?> GetFirstOrDefaultAsync(Expression<Func<TelegramBot, bool>> where)
    {
        return Db.TelegramBots.FirstOrDefaultAsync(where);
    }

    public Task<List<TelegramBot>> GetAllAsync()
    {
        return Db.TelegramBots.ToListAsync();
    }

    public async Task UpdateAsync(TelegramBot item)
    {
        var telegramBot = await Db.TelegramBots.FirstAsync(b => b.Id == item.Id).ConfigureAwait(false);
        Db.TelegramBots.Entry(telegramBot).CurrentValues.SetValues(item);
    }

    public async Task DeleteAsync(Guid id)
    {
        var telegramBot = await Db.TelegramBots.FirstAsync(b => b.Id == id).ConfigureAwait(false);
        Db.TelegramBots.Remove(telegramBot);
    }

    public Task<List<TelegramBot>> GetTelegramBotListAsync(Expression<Func<TelegramBot, bool>>? where = null)
    {
        return where is null ? Db.TelegramBots.ToListAsync() : Db.TelegramBots.Where(where).ToListAsync();
    }
}