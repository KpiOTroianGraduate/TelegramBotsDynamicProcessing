using System.Transactions;
using DAL;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Tests.Utils;

public class UnitOfWorkFactoryTest : IUnitOfWorkFactory
{
    private readonly TelegramContext _db;
    private readonly ILoggerFactory _loggerFactory;

    public UnitOfWorkFactoryTest(TelegramContext db, ILoggerFactory loggerFactory)
    {
        _db = db;
        _loggerFactory = loggerFactory;
    }

    public ISqlUnitOfWork CreateSqlUnitOfWork()
    {
        return new UnitOfWorkTest(_db, _loggerFactory);
    }

    public ISqlUnitOfWork CreateSqlUnitOfWork(IsolationLevel isolationLevel)
    {
        return new UnitOfWorkTest(_db, _loggerFactory);
    }
}