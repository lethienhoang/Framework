using Framework.CQRS.Dispatchers;
using Framework.CQRS.Handlers;
using Framework.ServiceBus.Infrastructure;
using System.Threading.Tasks;

namespace Framework.CronJobs
{
    public class SBQEventEnvelopeMessageHandler : IQueryHandler<SBQEventEnvelopeMessageRequest, int>
    {
        private readonly IDispatcher _dispatcher;

        public SBQEventEnvelopeMessageHandler(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public Task<int> HandleAsync(SBQEventEnvelopeMessageRequest query)
        {
            switch (query.Subject)
            {
                case "Demo":
                    //return _dispatcher.QueryAsync();
                    //return _dispatcher.SendAsync()
                    return 0;
                default:
                    return 0;
            }
        }
    }
}
