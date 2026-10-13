using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Domain.Common.Base;
using Tarefas.Domain.Common.Interfaces;
using Tarefas.Domain.Tarefas.Enums;

namespace Tarefas.Domain.Tarefas.Events;

public sealed record TarefaCriadaEvent(
    Guid TarefaId,
    string Titulo,
    string Descricao,
    PrioridadeTarefa Prioridade
) : DomainEventBase;



