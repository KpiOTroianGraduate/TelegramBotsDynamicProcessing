using Contracts.Entities;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories;

public class UserRepository : BaseRepository<TelegramContext, UserRepository>, IUserRepository
{
    public UserRepository(TelegramContext db, ILogger<UserRepository> logger) : base(db, logger)
    {
    }

    public async Task AddUserAsync()
    {
        await Db.Users.AddAsync(new User
        {
            Login = "hi"
        }).ConfigureAwait(false);
    }
}