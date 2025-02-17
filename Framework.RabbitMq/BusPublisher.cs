﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Framework.CQRS.Messages;
using Framework.Types;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Enrichers.MessageContext;

namespace Framework.RabbitMq
{
    public class BusPublisher : IBusPublisher
    {
        private readonly IBusClient _busClient;
        private readonly ILogger<IBusPublisher> _logger;
        private readonly string _defaultNamespace;

        public BusPublisher(IBusClient busClient, RabbitMqOptions options, ILogger<IBusPublisher> logger)
        {
            _busClient = busClient;
            _logger = logger;
            _defaultNamespace = options.Namespace;
        }

        public async Task PublishAsync<TEvent>(TEvent _event, ICorrelationContext context) where TEvent : IEvent
        {
            var eventName = _event.GetType().Name;
            _logger.LogInformation($"[Published an event] : '{eventName}'");

            await _busClient.PublishAsync(_event, ctx => ctx.UseMessageContext(context)
                .UsePublishConfiguration(p => p.WithRoutingKey(GetRoutingKey(_event))));
        }

        public async Task SendAsync<TCommand>(TCommand command, ICorrelationContext context) where TCommand : ICommand
        {
            var commandName = command.GetType().Name;
            _logger.LogInformation($"[Sent a command] : '{commandName}'"); ;

            await _busClient.PublishAsync(command, ctx => ctx.UseMessageContext(context)
                .UsePublishConfiguration(p => p.WithRoutingKey(GetRoutingKey(command))));   
        }

        private string GetRoutingKey<T>(T message)
        {
            var _namespace = message.GetType().GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace ?? _defaultNamespace;
            _namespace = string.IsNullOrWhiteSpace(_namespace) ? string.Empty : $"{_namespace}.";

            return $"{_namespace}{typeof(T).Name.Underscore()}".ToLowerInvariant();
        }
    }
}
