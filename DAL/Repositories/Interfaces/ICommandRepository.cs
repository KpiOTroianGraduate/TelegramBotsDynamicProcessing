using System.Linq.Expressions;
using Contracts.Entities;
using DAL.Repositories.Base;

namespace DAL.Repositories.Interfaces;

public interface ICommandRepository : IBaseRepository<Command>
{
    public Task<List<Command>> GetCommandsListAsync(Expression<Func<Command, bool>>? where = null);
}