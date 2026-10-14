using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Domain.Tarefas.Entities;
using Tarefas.Domain.Tarefas.Enums;
using Tarefas.Domain.Tarefas.Events;

namespace Tarefas.Domain.Tests.Tarefas.Entities;

public class TarefasTests
{
    [Fact(DisplayName = "Deve criar tarefa válida com status Pendente")]
    public void Deve_Criar_Tarefa_Valida()
    {
        //Act
        var tarefa = new Tarefa(
            "Estudar C#",
            "Estudar testes unitários",
            PrioridadeTarefa.Alta);

        //Assert
        tarefa.Should().NotBeNull();
        tarefa.Titulo.Should().Be("Estudar C#");
        tarefa.Descricao.Should().Be("Estudar testes unitários");
        tarefa.Status.Should().Be(StatusTarefa.Pendente);
        tarefa.Prioridade.Should().Be(PrioridadeTarefa.Alta);
        tarefa.Id.Should().NotBeEmpty();
        tarefa.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<TarefaCriadaEvent>();

    }
}

