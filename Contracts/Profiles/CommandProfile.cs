using AutoMapper;
using Contracts.Dto.Command;
using Contracts.Entities;

namespace Contracts.Profiles;

public class CommandProfile : Profile
{
    public CommandProfile()
    {
        CreateMap<Command, CommandReadDto>()
            .ReverseMap();


        CreateMap<Command, CommandDto>()
            .ReverseMap();
    }
}