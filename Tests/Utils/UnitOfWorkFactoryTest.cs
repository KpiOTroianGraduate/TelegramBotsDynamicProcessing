using DAL.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Logging;
using DAL;

namespace Tests.Utils
{
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
}
