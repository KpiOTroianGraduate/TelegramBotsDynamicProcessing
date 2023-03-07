using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Services.Base
{
    public class ServiceBase<T> where T : class
    {
        protected readonly IUnitOfWorkFactory UnitOfWorkFactory;
        protected readonly Logger<T> Logger;

        public ServiceBase(IUnitOfWorkFactory unitOfWorkFactory, Logger<T> logger)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            Logger = logger;
        }
    }
}
