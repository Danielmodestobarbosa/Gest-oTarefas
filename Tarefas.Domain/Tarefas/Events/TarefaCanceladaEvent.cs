using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Domain.Common.Base;

namespace Tarefas.Domain.Tarefas.Events;

public sealed record TarefaCanceladaEvent(
    Guid TarefaId
) : DomainEventBase;


