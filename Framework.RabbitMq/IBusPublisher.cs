using Framework.CQRS.Messages;
using Framework.Types;
using System.Threading.Tasks;

namespace Framework.RabbitMq
{
    public interface IBusPublisher
    {
        Task SendAsync<TCommand>(TCommand command, ICorrelationContext context)
            where TCommand : ICommand;

        Task PublishAsync<TEvent>(TEvent _sevent, ICorrelationContext context)
            where TEvent : IEvent;
    }
}
