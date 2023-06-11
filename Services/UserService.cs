using System.Security.Claims;
using System.Transactions;
using AutoMapper;
using Contracts.Dto.User;
using Contracts.Entities;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Base;
using Services.Interfaces;

namespace Services;

public class UserService : BaseService<UserService>, IUserService
{
    public UserService(IMapper mapper, IUnitOfWorkFactory unitOfWorkFactory, ILogger<UserService> logger) : base(mapper,
        unitOfWorkFactory, logger)
    {
    }

    public async Task<UserDto> RegisterUserAsync(IEnumerable<Claim> claims)
    {
        var user = Mapper.Map<User>(claims.ToArray());

        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork(IsolationLevel.ReadUncommitted);
        var dbUser = await unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email.Equals(user.Email))
            .ConfigureAwait(false);
        if (dbUser != null) return Mapper.Map<UserDto>(dbUser);

        await unitOfWork.UserRepository.AddAsync(user).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        dbUser = await unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Email.Equals(user.Email))
            .ConfigureAwait(false);
        return Mapper.Map<UserDto>(dbUser);
    }
}