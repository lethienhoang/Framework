using Framework.ServiceBus.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.CronJobs
{
    class QueueBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger<QueueBackgroundService> _logger;
        private readonly IServiceScope _serviceScope;
        private Timer _timer;

        public QueueBackgroundService(ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            _logger = loggerFactory.CreateLogger<QueueBackgroundService>();
            _serviceScope = serviceProvider.CreateScope();
        }

        private void DoWork(object state)
        {
            _logger.LogDebug("Starting Queue background service");

            var queue = _serviceScope.ServiceProvider.GetRequiredService<ISBQClient>();
            var handler = _serviceScope.ServiceProvider.GetRequiredService<SBQEventEnvelopeMessageHandler>();

            queue.RecieveMessageAsync((messge) =>
            {
                return handler.HandleAsync(messge).Result;
            });

            _logger.LogDebug("Queue background service started");
        }

        public void Dispose()
        {
            _timer.Dispose();
            _serviceScope.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1000));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queue Background Service is stopping.");

            this._timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
