using System.Security.Claims;
using AutoMapper;
using Contracts.Dto.TelegramBot;
using Contracts.Entities;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Base;
using Services.Interfaces;

namespace Services;

public class TelegramBotService : BaseService<TelegramBotService>, ITelegramBotService
{
    public TelegramBotService(IMapper mapper, IUnitOfWorkFactory unitOfWorkFactory, ILogger<TelegramBotService> logger)
        : base(mapper, unitOfWorkFactory, logger)
    {
    }

    public async Task CreateTelegramBotAsync(TelegramBotDto telegramBot)
    {
        var telegramBotEntity = Mapper.Map<TelegramBot>(telegramBot);

        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.TelegramBotRepository.AddAsync(telegramBotEntity).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<List<TelegramBot>> GetTelegramBotsAsync(IEnumerable<Claim> claims)
    {
        var email = claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;

        if (string.IsNullOrEmpty(email)) return new List<TelegramBot>();


        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        return await unitOfWork.TelegramBotRepository
            .GetTelegramBotListAsync(b => b.User.Email.Equals(email)).ConfigureAwait(false);
    }

    public async Task CreateTelegramBotAsync(IEnumerable<Claim> claims, TelegramBotDto telegramBot)
    {
        var email = claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;


        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        var user = await unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email.Equals(email))
            .ConfigureAwait(false);
        if (user == null) return;

        var telegramBotEntity = Mapper.Map<TelegramBot>(telegramBot);
        user.TelegramBots.Add(telegramBotEntity);
        await unitOfWork.UserRepository.UpdateAsync(user).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateTelegramBotAsync(Guid id, TelegramBotDto telegramBot)
    {
        var telegramBotEntity =
            Mapper.Map<TelegramBot>(telegramBot, opt => { opt.AfterMap((_, dest) => dest.Id = id); });

        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.TelegramBotRepository.UpdateAsync(telegramBotEntity).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteTelegramBotAsync(Guid id)
    {
        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.TelegramBotRepository.DeleteAsync(id).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}