using System;
using System.Threading.Tasks;

namespace Framework.ServiceBus.Infrastructure
{
    public interface ISBQClient
    {
        void RecieveMessageAsync(Func<SBQEventEnvelopeMessageRequest, int> eventHanlder);

        Task PublishMessageAsync(SBQEventEnvelopeMessageRequest requestBody);
    }
}
