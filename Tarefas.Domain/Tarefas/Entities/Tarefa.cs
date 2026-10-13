using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Domain.Common.Base;
using Tarefas.Domain.Common.Eexceptions;
using Tarefas.Domain.Common.Validations;
using Tarefas.Domain.Tarefas.Enums;
using Tarefas.Domain.Tarefas.Events;

namespace Tarefas.Domain.Tarefas.Entities;

public sealed class Tarefa : AggregateRoot
{
    public Guid TarefaId { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public StatusTarefa Status { get; private set; }
    public PrioridadeTarefa Prioridade { get; private set; }

    public Tarefa(
        string titulo,
        string descricao,
        PrioridadeTarefa prioridade)
    {
        Validar(titulo, prioridade);

        Titulo = titulo.Trim();
        Descricao = descricao?.Trim();
        Prioridade = prioridade;
        Status = StatusTarefa.Pendente;

        AddDomainEvent(new TarefaCriadaEvent(
            TarefaId: TarefaId,
            Titulo: Titulo,
            Descricao: Descricao,
            Prioridade: Prioridade));
    }

    public void Iniciar()
    {
        Guard.Against<DomainException>(
            Status != StatusTarefa.Pendente, 
            "A tarefa não pode ser iniciada pois não está pendente.");

        Status = StatusTarefa.EmAndamento;

        AddDomainEvent(new TarefaIniciadaEvent(
            TarefaId: TarefaId));

        SetDataAtualizacao();
    }

    public void Concluir()
    {
        if (Status == StatusTarefa.Concluida)
            return;

        Guard.Against<DomainException>(
            Status == StatusTarefa.Cancelada, 
            "A tarefa não pode ser concluída pois está cancelada.");

        Status = StatusTarefa.Concluida;
        DataConclusao = DateTime.UtcNow;

        AddDomainEvent(new TarefaConcluidaEvent(
            TarefaId: TarefaId,
            DataConclusao: DataConclusao.Value));

        SetDataAtualizacao();
    }

    public void Atualizar(
        string titulo,
        string? descricao,
        PrioridadeTarefa prioridade)
    {
        Guard.Against<DomainException>(
            Status == StatusTarefa.Cancelada, 
            "A tarefa não pode ser atualizada pois está cancelada.");

        Validar(titulo, prioridade);

        Titulo = titulo.Trim();
        Descricao = descricao?.Trim();
        Prioridade = prioridade;

        AddDomainEvent(new TarefaAtualizadaEvent(
            TarefaId: TarefaId,
            Titulo: Titulo,
            Prioridade: Prioridade));

        SetDataAtualizacao();
    }

    public void Cancelar()
    {
        Guard.Against<DomainException>(
            Status == StatusTarefa.Concluida,
            "A tarefa não pode ser cancelada pois está concluída.");

        Status = StatusTarefa.Cancelada;

        AddDomainEvent(new TarefaCanceladaEvent(
            TarefaId: TarefaId));

        SetDataAtualizacao();
    }

    public static void Validar(
        string titulo,
        PrioridadeTarefa prioridade)
    {
        Guard.AgainstNullOrWhiteSpace(
            titulo, nameof(titulo), 
            "O título da tarefa é obrigatório.");

        Guard.Against<DomainException>(
            titulo.Trim().Length < 3 ||
            titulo.Trim().Length > 100,
            "O título deve possuir entre 3 e 100 caracteres.");

        Guard.Against<DomainException>(
            !Enum.IsDefined(typeof(PrioridadeTarefa), prioridade),
            "A prioridade da tarefa é inválida.");
    }
}

