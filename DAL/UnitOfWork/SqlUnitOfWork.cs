using DAL.Repositories;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace DAL.UnitOfWork;

public class SqlUnitOfWork : ISqlUnitOfWork
{
    private readonly TelegramContext _db;
    private readonly ILoggerFactory _loggerFactory;

    private IUserRepository? _userRepository;

    public SqlUnitOfWork(TelegramContext db, ILoggerFactory loggerFactory)
    {
        _db = db;
        _loggerFactory = loggerFactory;
    }

    public IUserRepository UserRepository =>
        _userRepository ??= new UserRepository(_db, _loggerFactory.CreateLogger<UserRepository>());

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}