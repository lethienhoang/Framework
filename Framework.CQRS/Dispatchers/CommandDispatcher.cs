using System.Threading.Tasks;
using Framework.CQRS.Handlers;
using Framework.CQRS.Messages;
using Framework.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.CQRS.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;
        public CommandDispatcher(IServiceScopeFactory serviceScope)
        {
            _serviceFactory = serviceScope;
        }

        public async Task SendAsync<T>(T command) where T : ICommand
        {
            IServiceScope scopeService;
            using (var scope = _serviceFactory.CreateScope())
            {
                scopeService = scope;
            }

            var handler = scopeService.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
            await handler.HandleAsync(command, CorrelationContext.Empty);
        }
    }
}
