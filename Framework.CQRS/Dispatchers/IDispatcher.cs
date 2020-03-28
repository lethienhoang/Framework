using Framework.CQRS.Messages;
using Framework.Types;
using System.Threading.Tasks;

namespace Framework.CQRS.Dispatchers
{
    public interface IDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
