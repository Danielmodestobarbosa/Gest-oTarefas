using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Domain.Common.Eexceptions;
using Tarefas.Domain.Tarefas.Entities;
using Tarefas.Domain.Tarefas.Enums;
using Tarefas.Domain.Tarefas.Events;

namespace Tarefas.Domain.Tests.Tarefas.Entities;

public class TarefasTests
{
    #region Testes de Criação

    [Fact(DisplayName = "Deve criar tarefa válida com status Pendente")]
    public void Deve_Criar_Tarefa_Valida()
    {
        //Arrange
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

    [Fact(DisplayName = "Não deve criar tarefa com titulo menor que 3 caracteres")]
    public void Nao_Deve_Criar_Tarefa_Com_Titulo_Menor_Que_Tres_Caracteres()
    {
        //Arrange
        var titulo = "AB";
        var descricao = "Estudar testes unitários";
        var prioridade = PrioridadeTarefa.Alta;

        //Act 
        var acao = () => new Tarefa(titulo, descricao, prioridade);

        //Assert
        acao.Should().
            Throw<DomainException>().
            WithMessage("O título deve possuir entre 3 e 100 caracteres.");
    }

    [Fact(DisplayName = "Não deve criar tarefa com titulo maior que 100 caracteres")]
    public void Nao_Deve_Criar_Tarefa_Com_Titulo_Maior_Que_Cem_Caracteres()
    {
        //Arrange
        var titulo = new string('A', 101);
        var descricao = "Estudar testes unitarios";
        var prioridade = PrioridadeTarefa.Alta;

        //Act 
        var acao = () => new Tarefa(titulo, descricao, prioridade);

        //Assert
        acao.Should()
            .Throw<DomainException>()
            .WithMessage("O título deve possuir entre 3 e 100 caracteres.");
    }

    [Theory(DisplayName = "Não deve criar tarefa com titulo inválido")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Nao_Deve_Criar_Tarefa_Com_Titulo_Invalido(string? titulo)
    {
        //Arrange
        var descricao = "Estudar testes unitários";
        var prioridade = PrioridadeTarefa.Alta;

        //Act
        var acao = () => new Tarefa(titulo!, descricao, prioridade);

        //Assert
        acao.Should()
            .Throw<DomainException>()
            .WithMessage("O título da tarefa é obrigatório.");
    }

    [Fact(DisplayName = "Não deve criar tarefa com prioridade inválida")]
    public void Nao_Deve_Criar_Tarefa_Com_Prioridade_Invalida()
    {
        //Arrange
        var titulo = "Estudar C#";
        var descricao = "Estudar testes unitários";
        var prioridade = (PrioridadeTarefa)999;

        //Act
        var acao = () => new Tarefa(titulo, descricao, prioridade);

        //Assert
        acao.Should()
            .Throw<DomainException>()
            .WithMessage("A prioridade da tarefa é inválida.");
    }

    [Fact(DisplayName = "Deve criar tarefa com a descrição nula")]
    public void Criar_Tarefa_Com_Descricao_Nula()
    {
        //Arrange
        var tarefa = new Tarefa(
            "Estudar C#",
            null,
            PrioridadeTarefa.Alta);

        //Assert
        tarefa.Should().NotBeNull();
        tarefa.Titulo.Should().Be("Estudar C#");
        tarefa.Descricao.Should().BeNull();
        tarefa.Status.Should().Be(StatusTarefa.Pendente);
        tarefa.Prioridade.Should().Be(PrioridadeTarefa.Alta);
        tarefa.Id.Should().NotBeEmpty();
        tarefa.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<TarefaCriadaEvent>();

    }

    #endregion

    #region Testes de Iniciar Tarefa

    [Fact(DisplayName = "Deve iniciar uma tarefa pendente")]
    public void Deve_Iniciar_Tarefa_Pendente()
    {
        //Arrange 
        var tarefa = new Tarefa(
            "Estudar C#",
            "Estudar testes unitários",
            PrioridadeTarefa.Alta);

        //Act
        tarefa.Iniciar();

        //Assert
        tarefa.Status.Should().Be(StatusTarefa.EmAndamento);
    }

    [Fact(DisplayName = "Deve registrar evento ao iniciar tarefa")]
    public void Deve_Registrar_Evento_Ao_Iniciar_Tarefa()
    {
        //Arrange
        var tarefa = new Tarefa(
            "Estudar C#",
            "Estudar testes unitários",
            PrioridadeTarefa.Alta);

        //Act
        tarefa.Iniciar();

        //Assert
        tarefa.DomainEvents
            .Should()
            .ContainSingle(e => e is TarefaIniciadaEvent);
    }


    #endregion
}

