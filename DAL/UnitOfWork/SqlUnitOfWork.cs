using DAL.Repositories;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.UnitOfWork;

public class SqlUnitOfWork : ISqlUnitOfWork
{
    private readonly TelegramContext _db;
    private readonly ILoggerFactory _loggerFactory;
    private ICommandActionRepository? _commandActionRepository;
    private ICommandRepository? _commandRepository;
    private ITelegramBotRepository? _telegramBotRepository;

    private IUserRepository? _userRepository;

    public SqlUnitOfWork(ILoggerFactory loggerFactory)
    {
        //var connectionString = "Server=tcp:hapan9.database.windows.net,1433;Initial Catalog=hapan9-telegram;Persist Security Info=False;User ID=CloudSAacd16069;Password=61BimitE61;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TelegramBotDb;Trusted_Connection=True;";
        var sql = new DbContextOptionsBuilder<TelegramContext>()
            .UseSqlServer(connectionString);
        //var sql = new DbContextOptionsBuilder<TelegramContext>()
        //    .UseInMemoryDatabase("str");
        _db = new TelegramContext(sql.Options);
        _loggerFactory = loggerFactory;
    }

    public IUserRepository UserRepository =>
        _userRepository ??= new UserRepository(_db, _loggerFactory.CreateLogger<UserRepository>());

    public ITelegramBotRepository TelegramBotRepository => _telegramBotRepository ??=
        new TelegramBotRepository(_db, _loggerFactory.CreateLogger<TelegramBotRepository>());

    public ICommandRepository CommandRepository => _commandRepository ??=
        new CommandRepository(_db, _loggerFactory.CreateLogger<CommandRepository>());

    public ICommandActionRepository CommandActionRepository => _commandActionRepository ??=
        new CommandActionRepository(_db, _loggerFactory.CreateLogger<CommandActionRepository>());

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}