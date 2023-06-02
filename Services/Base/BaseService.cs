using AutoMapper;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Services.Base;

public class BaseService<T> where T : class
{
    protected readonly ILogger<T> Logger;
    protected readonly IMapper Mapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory;


    public BaseService(IMapper mapper, IUnitOfWorkFactory unitOfWorkFactory, ILogger<T> logger)
    {
        Mapper = mapper;
        UnitOfWorkFactory = unitOfWorkFactory;
        Logger = logger;
    }
}