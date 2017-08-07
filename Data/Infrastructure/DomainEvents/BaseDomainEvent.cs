using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infrastructure.DomainEvents
{
    public abstract class BaseDomainEvent
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
