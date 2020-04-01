using Framework.Types;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ServiceBus.Infrastructure
{
    public class SBQClient : ISBQClient
    {
        private readonly IQueueClient _client;
        private readonly ILogger<SBQClient> _logger;
        public SBQClient(IQueueClient client, ILogger<SBQClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public void RecieveMessageAsync(Func<SBQEventEnvelopeMessageRequest, int> eventHanlder)
        {
            try
            {
                _logger.LogDebug($"Queue starts on {DateTimeHelper.GenerateDateTime().ToLongTimeString()}");

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };

                _client.RegisterMessageHandler(async (message, cancellationToken) => 
                {
                    var payload = JsonConvert.DeserializeObject<SBQEventEnvelopeMessageRequest>(Encoding.UTF8.GetString(message.Body));
                    eventHanlder(payload);

                    await _client.CompleteAsync(message.SystemProperties.LockToken);
                }, messageHandlerOptions);

                _logger.LogDebug($"Queue end on {DateTimeHelper.GenerateDateTime().ToLongTimeString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception from RecieveMessageAsync");
                throw;
            }
        }

        public async Task PublishMessageAsync(SBQEventEnvelopeMessageRequest requestBody)
        {
            try
            {
                _logger.LogDebug($"Queue sending on {DateTimeHelper.GenerateDateTime().ToLongTimeString()}");

                var payload = JsonConvert.SerializeObject(requestBody);
                await _client.SendAsync(new Message(Encoding.UTF8.GetBytes(payload))
                {
                    MessageId = Guid.NewGuid().ToString(),
                });

                _logger.LogDebug($"Queue sent on {DateTimeHelper.GenerateDateTime().ToLongTimeString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception from PublishMessageAsync");
                throw;
            }
        }

        #region Setting up
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }
        #endregion
    }

}
