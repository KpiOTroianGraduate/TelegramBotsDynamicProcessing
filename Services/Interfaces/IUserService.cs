using System.Security.Claims;
using Contracts.Dto.User;
using Contracts.Entities;

namespace Services.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(Guid id);

    Task<UserDto> RegisterUserAsync(IEnumerable<Claim> claims);
}