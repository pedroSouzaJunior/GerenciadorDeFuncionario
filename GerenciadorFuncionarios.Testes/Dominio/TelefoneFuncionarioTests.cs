using System;
using GerenciadorFuncionarios.Dominio.Entities;
using Xunit;

namespace GerenciadorFuncionarios.Testes.Dominio
{
    public class TelefoneFuncionarioTests
    {
        [Fact]
        public void TelefoneFuncionario_DeveSerCriadoCorretamente()
        {
            // Arrange
            var telefone = new TelefoneFuncionario
            {
                Id = 1,
                Numero = "11999999999",
                FuncionarioId = 2
            };

            // Assert
            Assert.Equal(1, telefone.Id);
            Assert.Equal("11999999999", telefone.Numero);
            Assert.Equal(2, telefone.FuncionarioId);
        }

        [Fact]
        public void TelefoneFuncionario_NumeroNaoDeveSerNulo()
        {
            // Arrange
            var telefone = new TelefoneFuncionario { Numero = null };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => telefone.Numero.ToString());
        }

        [Fact]
        public void TelefoneFuncionario_DeveTerFuncionarioAssociado()
        {
            // Arrange
            var funcionario = new Funcionario { Id = 1, Nome = "Carlos" };
            var telefone = new TelefoneFuncionario { Id = 1, Numero = "11999999999", Funcionario = funcionario };

            // Assert
            Assert.NotNull(telefone.Funcionario);
            Assert.Equal(funcionario.Id, telefone.Funcionario.Id);
            Assert.Equal("Carlos", telefone.Funcionario.Nome);
        }

        [Fact]
        public void TelefoneFuncionario_FuncionarioDeveSerNuloPorPadrao()
        {
            // Arrange
            var telefone = new TelefoneFuncionario();

            // Assert
            Assert.Null(telefone.Funcionario);
        }
    }
}
