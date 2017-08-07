using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infrastructure.DomainEvents
{
    public interface IHandle<T> where T : BaseDomainEvent
    {
        void Handle(T domainEvent);
    }
}
