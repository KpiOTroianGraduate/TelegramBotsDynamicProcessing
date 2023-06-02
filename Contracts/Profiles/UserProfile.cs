using AutoMapper;
using Contracts.Dto.User;
using Contracts.Entities;

namespace Contracts.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}