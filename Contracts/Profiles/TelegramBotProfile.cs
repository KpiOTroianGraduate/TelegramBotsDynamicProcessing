using AutoMapper;
using Contracts.Dto.TelegramBot;
using Contracts.Entities;
using User = Telegram.Bot.Types.User;

namespace Contracts.Profiles;

public class TelegramBotProfile : Profile
{
    public TelegramBotProfile()
    {
        CreateMap<TelegramBot, TelegramBotDto>()
            .ReverseMap();

        CreateMap<User, TelegramBotInfoDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}