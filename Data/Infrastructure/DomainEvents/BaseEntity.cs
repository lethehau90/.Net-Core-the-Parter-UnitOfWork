using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infrastructure.DomainEvents
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
