using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Domain.Common.Interfaces;

namespace Tarefas.Domain.Common.Base;
public abstract record class DomainEventBase : IDomainEvent
{
    public DateTime DateOcurred { get; protected set; } = DateTime.UtcNow;
}
