using Framework.CQRS.Messages;
using Framework.Types;
using System.Threading.Tasks;

namespace Framework.CQRS.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent _event, ICorrelationContext context);
    }
}
