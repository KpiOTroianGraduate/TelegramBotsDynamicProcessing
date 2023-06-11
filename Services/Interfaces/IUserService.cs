using System.Security.Claims;
using Contracts.Dto.User;

namespace Services.Interfaces;

public interface IUserService
{
    Task<UserDto> RegisterUserAsync(IEnumerable<Claim> claims);
}