using FluentAssertions;
using Tarefas.Domain.Common.Eexceptions;
using Tarefas.Domain.Usuarios.Entities;
using Tarefas.Domain.Usuarios.Enums;
using Tarefas.Domain.Usuarios.Events;
using Tarefas.Domain.Usuarios.ValueObjects;

namespace Tarefas.Domain.Tests.Usuarios.Entities;

public class UsuarioTests
{
    #region Testes de Criação do usuário

    [Fact(DisplayName = "Deve criar usuário válido com status ativo")]
    public void Deve_Criar_Usuario_Valido_Com_Status_Ativo()
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");
        var email = new Email("daniel@hotmail.com");

        //Act
        var usuario = new Usuario(nome, email);

        // Assert
        usuario.Should().NotBeNull();
        usuario.Nome.Should().Be(nome);
        usuario.Email.Should().Be(email);
        usuario.Status.Should().Be(StatusUsuario.Ativo);
        usuario.Id.Should().NotBeEmpty();
        usuario.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UsuarioCriadoEvent>();
    }

    [Fact(DisplayName = "Deve registrar evento ao criar usuário")]
    public void Deve_Registrar_Evento_Ao_Criar_Usuario()
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");
        var email = new Email("daniel@hotmail.com");

        //Act
        var usuario = new Usuario(nome, email);

        //Assert
        usuario.DomainEvents
            .Should()
            .ContainSingle(u => u is UsuarioCriadoEvent);
    }

    [Theory(DisplayName = "Não deve criar usuário com nome nulo")]
    [InlineData(null)]
    public void Nao_Deve_Criar_Usuario_Com_Nome_Nulo(string? nome)
    {
        //Arrange
        var email = new Email("daniel@hotmail.com");

        //Act
        var acao = () => new Usuario(
        new NomeUsuario(nome!),
        email);

        //Assert
        acao.Should()
            .Throw<DomainException>()
            .WithMessage("O nome do usuário não pode ser nulo ou vazio.");
    }

    [Theory(DisplayName = "Não deve criar usuário com email nulo")]
    [InlineData(null)]
    public void Nao_Deve_Criar_Usuario_Com_Email_Nulo(string? email)
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");

        //Act
        var acao = () => new Usuario(
        nome,
        new Email(email!));

        //Assert
        acao.Should()
            .Throw<DomainException>()
            .WithMessage("O endereço de email é obrigatório");
    }




    #endregion

    #region Testes de Atualizar perfil

    [Fact(DisplayName = "Deve atualizar nome e email")]
    public void Deve_Atualizar_Nome_E_Email()
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");
        var email = new Email("daniel@hotmail.com");

        var usuario = new Usuario(nome, email);

        var novoNome = new NomeUsuario("Daniel Barbosa");
        var novoEmail = new Email("danielmodestop@hotmail.com");

        //Act 
        usuario.AtualizarPerfil(novoNome, novoEmail);

        //Assert
        usuario.Nome.Nome.Should().Be("Daniel Barbosa");
        usuario.Email.Endereco.Should().Be("danielmodestop@hotmail.com");
    }

    [Fact(DisplayName = "Deve registrar evento ao atualizar usuário")]
    public void Deve_Registrar_Evento_Ao_Atualizar_Usuario()
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");
        var email = new Email("daniel@hotmail.com");

        var usuario = new Usuario(nome, email);

        var novoNome = new NomeUsuario("Daniel Barbosa");
        var novoEmail = new Email("danielmodestop@hotmail.com");

        //Act 
        usuario.AtualizarPerfil(novoNome, novoEmail);

        //Assert
        usuario.DomainEvents
            .Should()
            .ContainSingle(u => u is UsuarioAtualizadoEvent);
    }

    [Fact(DisplayName = "Deve atualizar DataAtualizacao")]
    public void Deve_Atualizar_DataAtualizacao()
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");
        var email = new Email("daniel@hotmail.com");

        var usuario = new Usuario(nome, email);

        var novoNome = new NomeUsuario("Daniel Barbosa");
        var novoEmail = new Email("danielmodestop@hotmail.com");

        //Act 
        usuario.AtualizarPerfil(novoNome, novoEmail);

        //Assert
        usuario.DataAtualizacao.Should().NotBeNull();
    }

    [Fact(DisplayName = "Não deve atualizar perfil de usuário inativo")]
    public void Nao_Deve_Atualizar_Perfil_De_Usuario_Inativo()
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");
        var email = new Email("daniel@hotmail.com");

        var usuario = new Usuario(nome, email);

        usuario.DesativarPerfil();

        var novoNome = new NomeUsuario("Daniel Barbosa");
        var novoEmail = new Email("danielmodestop@hotmail.com");

        //Act 
        var acao = () => usuario.AtualizarPerfil(novoNome, novoEmail);

        //Assert
        acao.Should()
            .Throw<DomainException>()
            .WithMessage("Usuários inativos não podem ter o perfil atualizado");

    }

    [Fact(DisplayName = "Não deve atualizar perfil do usuário com nome nulo")]
    public void Nao_Deve_Atualizar_Pefil_De_Usuario_Com_Nome_Nulo()
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");
        var email = new Email("daniel@hotmail.com");

        var usuario = new Usuario(nome, email);

        var novoEmail = new Email("danielmodestop@hotmail.com");

        //Act 
        var acao = () => usuario.AtualizarPerfil(null!, novoEmail);

        //Assert
        acao.Should()
            .Throw<DomainException>()
            .WithMessage("O parâmetro 'nome' não pode ser nulo.");
    }

    [Fact(DisplayName = "Não deve atualizar perfil do usuário com email nulo")]
    public void Nao_Deve_Atualizar_Pefil_De_Usuario_Com_Email_Nulo()
    {
        //Arrange
        var nome = new NomeUsuario("Daniel Modesto");
        var email = new Email("daniel@hotmail.com");

        var usuario = new Usuario(nome, email);

        var novoNome = new NomeUsuario("Daniel Barbosa");
        //Act 
        var acao = () => usuario.AtualizarPerfil(novoNome, null!);

        //Assert
        acao.Should()
            .Throw<DomainException>()
            .WithMessage("O parâmetro 'email' não pode ser nulo.");
    }

    #endregion

    #region Testes de Desativar usuário



    #endregion

    #region Testes de Ativar usuário



    #endregion
}
