using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Domain.Common.Base;
using Tarefas.Domain.Usuarios.ValueObjects;

namespace Tarefas.Domain.Usuarios.Events;

public sealed record UsuarioReativadoEvent(
    Guid UsuarioId) : DomainEventBase;


