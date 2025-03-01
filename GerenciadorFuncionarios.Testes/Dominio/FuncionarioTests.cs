using System;
using System.Collections.Generic;
using GerenciadorFuncionarios.Dominio.Entities;
using Xunit;

namespace GerenciadorFuncionarios.Testes.Dominio
{
    public class FuncionarioTests
    {
        [Fact]
        public void Funcionario_DeveSerCriadoCorretamente()
        {
            // Arrange
            var funcionario = new Funcionario
            {
                Id = 1,
                Nome = "Carlos",
                Sobrenome = "Silva",
                Email = "carlos@email.com",
                Documento = "12345678900",
                DataNascimento = new DateTime(1990, 5, 20),
                SenhaHash = "senha_hash",
                Role = "Admin"
            };

            // Assert
            Assert.Equal(1, funcionario.Id);
            Assert.Equal("Carlos", funcionario.Nome);
            Assert.Equal("Silva", funcionario.Sobrenome);
            Assert.Equal("carlos@email.com", funcionario.Email);
            Assert.Equal("12345678900", funcionario.Documento);
            Assert.Equal(new DateTime(1990, 5, 20), funcionario.DataNascimento);
            Assert.Equal("senha_hash", funcionario.SenhaHash);
            Assert.Equal("Admin", funcionario.Role);
        }

        [Fact]
        public void Funcionario_DeveIniciarComListaDeTelefonesVazia()
        {
            // Arrange
            var funcionario = new Funcionario();

            // Assert
            Assert.NotNull(funcionario.Telefones);
            Assert.Empty(funcionario.Telefones);
        }

        [Fact]
        public void Funcionario_DeveAdicionarTelefoneCorretamente()
        {
            // Arrange
            var funcionario = new Funcionario();
            var telefone = new TelefoneFuncionario { Id = 1, Numero = "11999999999" };

            // Act
            funcionario.Telefones.Add(telefone);

            // Assert
            Assert.Single(funcionario.Telefones);
            Assert.Equal("11999999999", funcionario.Telefones[0].Numero);
        }

        [Fact]
        public void Funcionario_DeveTerGestorAtribuido()
        {
            // Arrange
            var gestor = new Funcionario { Id = 2, Nome = "Gestor" };
            var funcionario = new Funcionario { Id = 1, Nome = "Carlos", Gestor = gestor, GestorId = gestor.Id };

            // Assert
            Assert.NotNull(funcionario.Gestor);
            Assert.Equal(gestor.Id, funcionario.GestorId);
            Assert.Equal("Gestor", funcionario.Gestor.Nome);
        }

        [Fact]
        public void Funcionario_GestorDeveSerNuloPorPadrao()
        {
            // Arrange
            var funcionario = new Funcionario();

            // Assert
            Assert.Null(funcionario.Gestor);
            Assert.Null(funcionario.GestorId);
        }

        [Fact]
        public void Funcionario_DeveTerSenhaObrigatoria()
        {
            // Arrange
            var funcionario = new Funcionario { SenhaHash = null };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => funcionario.SenhaHash.ToString());
        }
    }
}
