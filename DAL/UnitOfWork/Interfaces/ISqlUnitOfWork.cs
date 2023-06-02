using DAL.Repositories.Interfaces;

namespace DAL.UnitOfWork.Interfaces;

public interface ISqlUnitOfWork
{
    IUserRepository UserRepository { get; }
    ITelegramBotRepository TelegramBotRepository { get; }
    ICommandRepository CommandRepository { get; }
    ICommandActionRepository CommandActionRepository { get; }

    Task SaveChangesAsync();
}