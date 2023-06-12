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

        CreateMap<string, TelegramBotDescriptionDto>()
            .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore());
    }
}