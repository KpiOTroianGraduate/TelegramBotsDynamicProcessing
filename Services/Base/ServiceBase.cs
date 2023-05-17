using Microsoft.Extensions.Logging;

namespace Services.Base;

public class ServiceBase<T> where T : class
{
    protected readonly ILogger<T> Logger;

    public ServiceBase(ILogger<T> logger)
    {
        Logger = logger;
    }
}