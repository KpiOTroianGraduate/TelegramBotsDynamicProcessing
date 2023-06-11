using AutoMapper;
using Contracts.Profiles;
using DAL.UnitOfWork;
using DAL.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(setup =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });

        builder.Services.AddTransient<ITelegramService, TelegramService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
        builder.Services.AddTransient<ITelegramBotService, TelegramBotService>();
        builder.Services.AddTransient<ICommandService, CommandService>();
        builder.Services.AddTransient<ICommandActionService, CommandActionService>();
        builder.Services.AddTransient<IVerifyService, VerifyService>();
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