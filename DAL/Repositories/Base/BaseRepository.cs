using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories.Base;

public abstract class BaseRepository<T, TA> where T : DbContext where TA : class
{
    protected readonly T Db;
    protected readonly ILogger<TA> Logger;

    protected BaseRepository(T db, ILogger<TA> logger)
    {
        Db = db;
        Logger = logger;
    }
}