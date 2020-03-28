using Framework.Types;
using System;

namespace Framework.Domain
{
    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; protected set; }
        public DateTime Created { get; protected set; }
        public DateTime? Updated { get; protected set; }

        protected EntityBase()
        {
        }

        protected EntityBase(Guid id)
        {
            Id = id;
            Created = DateTimeHelper.GenerateTodayUTC();
        }
    }
}
