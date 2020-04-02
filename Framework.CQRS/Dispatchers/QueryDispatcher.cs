using System;
using System.Threading.Tasks;
using Framework.CQRS.Handlers;
using Framework.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.CQRS.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;
        public QueryDispatcher(IServiceScopeFactory serviceScope)
        {
            _serviceFactory = serviceScope;
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            IServiceScope scopeService;
            using (var scope = _serviceFactory.CreateScope())
            {
                scopeService = scope;
            }

            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = scopeService.ServiceProvider.GetRequiredService(handlerType);

            return await handler.HandleAsync((dynamic)query);
        }
    }
}
