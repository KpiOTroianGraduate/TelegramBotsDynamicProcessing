using DAL;
using DAL.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Utils;
using UI;

namespace Tests.IntegrationTests.Utils;

public class ApplicationFixture
{
    public ApplicationFixture()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var options = new DbContextOptionsBuilder<TelegramContext>()
                        .UseInMemoryDatabase("InMemoryDd")
                        .Options;
                    services.AddScoped(_ => new TelegramContext(options));
                    services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactoryTest>();
                });
            });

        Client = application.CreateClient();
    }


    public HttpClient Client { get; }
    public TelegramContext TelegramContext { get; set; }

    public void Dispose()
    {
        Client.Dispose();
    }
}