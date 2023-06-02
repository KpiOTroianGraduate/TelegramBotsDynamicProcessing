using System.Linq.Expressions;
using Contracts.Entities;
using DAL.Repositories.Base;

namespace DAL.Repositories.Interfaces;

public interface ICommandActionRepository : IBaseRepository<CommandAction>
{
    public Task<List<CommandAction>> GetCommandActionListAsync(Expression<Func<CommandAction, bool>>? where = null);
}