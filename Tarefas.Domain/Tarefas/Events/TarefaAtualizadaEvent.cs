using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Domain.Common.Base;
using Tarefas.Domain.Tarefas.Enums;

namespace Tarefas.Domain.Tarefas.Events;

public sealed record TarefaAtualizadaEvent(
    Guid TarefaId,
    string Titulo,
    PrioridadeTarefa Prioridade
) : DomainEventBase;


