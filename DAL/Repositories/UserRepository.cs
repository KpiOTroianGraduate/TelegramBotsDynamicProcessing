using System.Linq.Expressions;
using Contracts.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories;

public sealed class UserRepository : BaseRepository<ITelegramContext, UserRepository>, IUserRepository
{
    public UserRepository(ITelegramContext db, ILogger<UserRepository> logger) : base(db, logger)
    {
    }

    public async Task AddAsync(User item)
    {
        await Db.Users.AddAsync(item).ConfigureAwait(false);
    }

    public Task<User> GetAsync(Guid id)
    {
        return Db.Users.FirstAsync(u => u.Id == id);
    }

    public Task<User?> GetFirstOrDefaultAsync(Expression<Func<User, bool>> where)
    {
        return Db.Users.FirstOrDefaultAsync(where);
    }

    public Task<List<User>> GetAllAsync()
    {
        return Db.Users.ToListAsync();
    }

    public async Task UpdateAsync(User item)
    {
        var user = await Db.Users.FirstAsync(u => u.Id == item.Id).ConfigureAwait(false);
        Db.Users.Entry(user).CurrentValues.SetValues(item);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await Db.Users.FirstAsync(u => u.Id == id).ConfigureAwait(false);
        Db.Users.Remove(user);
    }
}