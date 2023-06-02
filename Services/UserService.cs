using System.Security.Claims;
using AutoMapper;
using Contracts.Dto.User;
using Contracts.Entities;
using Contracts.Exceptions;
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

    public async Task<UserDto> GetUserByIdAsync(IEnumerable<Claim> claims, Guid id)
    {
        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        var user = await unitOfWork.UserRepository.GetAsync(id).ConfigureAwait(false);

        return user == null ? throw new NotFoundException() : Mapper.Map<UserDto>(user);
    }

    public async Task CreateUserAsync(IEnumerable<Claim> claims)
    {
        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        await unitOfWork.UserRepository.AddAsync(new User
        {
            Email = "email",
            FirstName = "firstName",
            Surname = "lastName"
        }).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        //var result = await unitOfWork.CommandActionRepository.GetAsync(Guid.Parse("19ABF8A8-C63E-497B-3143-08DB5BCCBB7D"))
        //    .ConfigureAwait(false);
        //var res = JsonConvert.DeserializeObject<KeyboardMarkupDto<InlineKeyboardButton>>(result.Content);
        //return;

        //var claimsArray = claims.ToArray();
        //var user = Mapper.Map<User>(claimsArray);

        //var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork();
        //await unitOfWork.UserRepository.AddAsync(user).ConfigureAwait(false);
        //await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}