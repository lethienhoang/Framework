using Newtonsoft.Json;
using System;

namespace Framework.Types
{
    public class CorrelationContext : ICorrelationContext
    {
        public Guid Id { get; }

        public Guid UserId { get; }

        public Guid ResourceId { get; }

        public string SpanContext { get; }

        public string ConnectionId { get; }

        public string Name { get; }

        public string Origin { get; }

        public string Resource { get; }

        public string Culture { get; }

        public DateTime CreateAt { get; }

        public int Retries { get; set; }

        public CorrelationContext()
        {
        }

        public CorrelationContext(Guid id)
        {
            Id = id;
        }

        [JsonConstructor]
        private CorrelationContext(Guid id, Guid userId, Guid resourceId, 
            string spanContext, string connectionId, string name, string origin, 
            string culture, string resource, int retries)
        {
            Id = id;
            UserId = userId;
            ResourceId = resourceId;
            SpanContext = spanContext;
            ConnectionId = connectionId;
            Name = string.IsNullOrWhiteSpace(name) ? string.Empty : GetName(name);
            Origin = string.IsNullOrWhiteSpace(origin) ? string.Empty :
                origin.StartsWith("/") ? origin.Remove(0, 1) : origin;
            Culture = culture;
            Resource = resource;
            Retries = retries;
            CreateAt = DateTime.UtcNow;
        }

        public static ICorrelationContext Empty
            => new CorrelationContext();

        public static ICorrelationContext FromId(Guid id)
            => new CorrelationContext(id);

        public static ICorrelationContext From<T>(ICorrelationContext context)
            => Create<T>(context.Id, context.UserId, context.ResourceId, context.ConnectionId,
                context.Origin, context.Culture, context.Resource);

        public static ICorrelationContext Create<T>(Guid id, Guid userId, Guid resourceId, string origin,
            string spanContext, string connectionId, string culture, string resource = "")
            => new CorrelationContext(id, userId, resourceId, spanContext, connectionId, typeof(T).Name, origin, culture,
                resource, 0);

        private static string GetName(string name)
            => name.Underscore().ToLowerInvariant();
    }
}
