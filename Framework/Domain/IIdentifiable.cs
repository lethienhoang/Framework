using System;

namespace Framework.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}
