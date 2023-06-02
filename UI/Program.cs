using AutoMapper;
using Contracts.Profiles;
using DAL;
using DAL.UnitOfWork;
using DAL.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Services;
using Services.Interfaces;

namespace UI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();


        builder.Services.AddControllers().AddNewtonsoftJson();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<TelegramContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("TelegramConnection");
            connectionString =
                "Server=tcp:hapan9.database.windows.net,1433;Initial Catalog=hapan9-telegram;Persist Security Info=False;User ID=CloudSAacd16069;Password=61BimitE61;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            options.UseSqlServer(connectionString);
        });

        builder.Services.AddTransient<ITelegramService, TelegramService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
        builder.Services.AddTransient<ITelegramBotService, TelegramBotService>();
        builder.Services.AddTransient<ICommandService, CommandService>();
        builder.Services.AddTransient<ICommandActionService, CommandActionService>();
        builder.Services.AddHttpClient();
        builder.Services.AddAutoMapper(m => m.AddProfiles(new List<Profile>
        {
            new ClaimProfile(),
            new UserProfile(),
            new TelegramBotProfile(),
            new CommandProfile(),
            new CommandActionProfile()
        }));


        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseStaticFiles();
        app.UseHttpsRedirection();


        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapGet("/", context => Task.Run(() => context.Response.Redirect("index.html")));
        app.UseCookiePolicy();
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials());


        app.MapControllers();


        app.Run();
    }
}