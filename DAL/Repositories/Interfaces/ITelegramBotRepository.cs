using System.Linq.Expressions;
using Contracts.Entities;
using DAL.Repositories.Base;

namespace DAL.Repositories.Interfaces;

public interface ITelegramBotRepository : IBaseRepository<TelegramBot>
{
    public Task<List<TelegramBot>> GetTelegramBotListAsync(Expression<Func<TelegramBot, bool>>? where = null);
}