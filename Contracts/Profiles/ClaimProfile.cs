using System.Security.Claims;
using AutoMapper;
using Contracts.Dto.User;
using Contracts.Entities;

namespace Contracts.Profiles;

public class ClaimProfile : Profile
{
    public ClaimProfile()
    {
        CreateMap<Claim[], User>()
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.First(x => x.Type.Equals("preferred_username")).Value))
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.First(x => x.Type.Equals("name")).Value));

        CreateMap<Claim[], UserDto>()
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.First(x => x.Type == ClaimTypes.Email).Value))
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.First(x => x.Type == ClaimTypes.Name).Value))
            .ForMember(dest => dest.Surname,
                opt => opt.MapFrom(src => src.First(x => x.Type == ClaimTypes.Surname).Value));
    }
}