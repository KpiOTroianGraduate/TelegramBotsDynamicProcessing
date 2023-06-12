using System.Security.Claims;
using System.Transactions;
using AutoMapper;
using Contracts.Entities;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Base;
using Services.Interfaces;

namespace Services;

public sealed class VerifyService : BaseService<VerifyService>, IVerifyService
{
    public VerifyService(IMapper mapper, IUnitOfWorkFactory unitOfWorkFactory, ILogger<VerifyService> logger) : base(
        mapper, unitOfWorkFactory, logger)
    {
    }

    public async Task<bool> VerifyTelegramBotAsync(IEnumerable<Claim> claims, Guid id)
    {
        var user = Mapper.Map<User>(claims.ToArray());


        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork(IsolationLevel.ReadUncommitted);
        var dbUser = await unitOfWork.UserRepository
            .GetFirstOrDefaultAsync(u => u.Email.Equals(user.Email) && u.TelegramBots.Any(b => b.Id == id))
            .ConfigureAwait(false);

        return dbUser != null;
    }

    public async Task<bool> VerifyCommandAsync(IEnumerable<Claim> claims, Guid id)
    {
        var user = Mapper.Map<User>(claims.ToArray());


        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork(IsolationLevel.ReadUncommitted);
        var dbUser = await unitOfWork.UserRepository.GetFirstOrDefaultAsync(u =>
                u.Email.Equals(user.Email) && u.TelegramBots.Select(b => b.Commands)
                    .Any(commands => commands.Any(c => c.Id == id)))
            .ConfigureAwait(false);

        return dbUser != null;
    }

    public async Task<bool> VerifyCommandActionAsync(IEnumerable<Claim> claims, Guid id)
    {
        var user = Mapper.Map<User>(claims.ToArray());


        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork(IsolationLevel.ReadUncommitted);
        var dbUser = await unitOfWork.UserRepository.GetFirstOrDefaultAsync(u =>
                u.Email.Equals(user.Email) && u.TelegramBots.Select(b => b.CommandActions)
                    .Any(actions => actions.Any(a => a.Id == id)))
            .ConfigureAwait(false);

        return dbUser != null;
    }
}