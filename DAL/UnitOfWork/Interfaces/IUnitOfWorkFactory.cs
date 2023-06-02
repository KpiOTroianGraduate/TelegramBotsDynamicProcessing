using System.Transactions;

namespace DAL.UnitOfWork.Interfaces;

public interface IUnitOfWorkFactory
{
    ISqlUnitOfWork CreateSqlUnitOfWork();
    ISqlUnitOfWork CreateSqlUnitOfWork(IsolationLevel isolationLevel);
}