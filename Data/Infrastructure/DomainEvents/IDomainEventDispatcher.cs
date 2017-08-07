using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infrastructure.DomainEvents
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
    }
}
