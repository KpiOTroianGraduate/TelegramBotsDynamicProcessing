using DAL;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Tests.Utils;

public class UnitOfWorkTest : ISqlUnitOfWork
{
    private readonly TelegramContext _db;
    private readonly ILoggerFactory _loggerFactory;

    public UnitOfWorkTest(TelegramContext db, ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _db = db;
    }

    public IUserRepository UserRepository => new UserRepository(_db, _loggerFactory.CreateLogger<UserRepository>());

    public ITelegramBotRepository TelegramBotRepository =>
        new TelegramBotRepository(_db, _loggerFactory.CreateLogger<TelegramBotRepository>());

    public ICommandRepository CommandRepository =>
        new CommandRepository(_db, _loggerFactory.CreateLogger<CommandRepository>());

    public ICommandActionRepository CommandActionRepository =>
        new CommandActionRepository(_db, _loggerFactory.CreateLogger<CommandActionRepository>());

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}