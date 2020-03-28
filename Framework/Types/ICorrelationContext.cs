using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Types
{
    public interface ICorrelationContext
    {
        Guid Id { get; }

        Guid UserId { get; }

        Guid ResourceId { get; }

        string SpanContext { get; }

        string ConnectionId { get; }

        string Name { get; }

        string Origin { get; }

        string Resource { get; }

        string Culture { get; }

        DateTime CreateAt { get; }

        int Retries { get; set; }
    }
}
