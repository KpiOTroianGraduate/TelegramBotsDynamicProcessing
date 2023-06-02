using System.Security.Claims;
using Contracts.Dto.User;

namespace Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(IEnumerable<Claim> claims, Guid id);

    Task CreateUserAsync(IEnumerable<Claim> claims);
}