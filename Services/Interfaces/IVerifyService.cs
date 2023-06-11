using System.Security.Claims;

namespace Services.Interfaces;

public interface IVerifyService
{
    Task<bool> VerifyTelegramBotAsync(IEnumerable<Claim> claims, Guid id);
    Task<bool> VerifyCommandAsync(IEnumerable<Claim> claims, Guid id);
    Task<bool> VerifyCommandActionAsync(IEnumerable<Claim> claims, Guid id);
}