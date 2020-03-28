using Framework.CQRS.Messages;
using Framework.Types;
using System.Threading.Tasks;

namespace Framework.CQRS.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext context);
    }
}
