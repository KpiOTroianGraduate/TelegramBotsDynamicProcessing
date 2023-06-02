using DAL;
using DAL.UnitOfWork;
using DAL.UnitOfWork.Interfaces;
using UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tests.Utils;


namespace Tests.IntegrationTests.Utils
{
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
            var scope = application.Services.CreateScope().ServiceProvider;
        }


        public HttpClient Client { get; }
        public TelegramContext TelegramContext { get; set; }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
