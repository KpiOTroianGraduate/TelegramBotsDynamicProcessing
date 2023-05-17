using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork.Interfaces;

public interface ISqlUnitOfWork
{
    IUserRepository UserRepository { get; }

    Task SaveChangesAsync();
}