using System;
using System.Collections.Generic;
using System.Text;

namespace Tarefas.Domain.Common.Interfaces
{
    public interface IDomainEvent
    {
        DateTime DateOcurred { get; }
    }
}
