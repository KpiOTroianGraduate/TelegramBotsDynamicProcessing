using AutoMapper;
using Contracts.Dto;
using Contracts.Entities;
using Contracts.Profiles.Resolvers;

namespace Contracts.Profiles;

public class CommandActionProfile : Profile
{
    public CommandActionProfile()
    {
        CreateMap<CommandAction, CommandActionDto>()
            .ReverseMap();

        CreateMap<CommandAction, CommandActionOptionDto>()
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(new CommandActionOptionTitleResolver()));
    }
}