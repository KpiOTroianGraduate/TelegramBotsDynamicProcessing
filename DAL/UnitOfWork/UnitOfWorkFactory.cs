using System.Transactions;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace DAL.UnitOfWork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public UnitOfWorkFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public ISqlUnitOfWork CreateSqlUnitOfWork()
    {
        return new SqlUnitOfWork(_loggerFactory);
    }

    public ISqlUnitOfWork CreateSqlUnitOfWork(IsolationLevel isolationLevel)
    {
        using var scope = new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = isolationLevel });
        return new SqlUnitOfWork(_loggerFactory);
    }
}