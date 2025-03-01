using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using GerenciadorFuncionarios.Aplicacao.DTOs;
using GerenciadorFuncionarios.Aplicacao.Interfaces;
using GerenciadorFuncionarios.Aplicacao.Services;
using GerenciadorFuncionarios.Dominio.Entities;
using BCrypt.Net;

namespace GerenciadorFuncionarios.Testes
{
    public class FuncionarioServicoTests
    {
        private readonly Mock<IFuncionarioRepositorio> _funcionarioRepositorioMock;
        private readonly FuncionarioServico _funcionarioServico;

        public FuncionarioServicoTests()
        {
            _funcionarioRepositorioMock = new Mock<IFuncionarioRepositorio>();
            _funcionarioServico = new FuncionarioServico(_funcionarioRepositorioMock.Object);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarListaDeFuncionarios()
        {
            // Arrange
            var funcionarios = new List<Funcionario>
            {
                new Funcionario { Id = 1, Nome = "João", Email = "joao@email.com", Telefones = new List<TelefoneFuncionario>() },
                new Funcionario { Id = 2, Nome = "Maria", Email = "maria@email.com", Telefones = new List<TelefoneFuncionario>() }
            };

            _funcionarioRepositorioMock.Setup(repo => repo.ObterTodosAsync()).ReturnsAsync(funcionarios);

            // Act
            var resultado = await _funcionarioServico.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarFuncionario_QuandoExistir()
        {
            // Arrange
            var funcionario = new Funcionario { Id = 1, Nome = "João", Email = "joao@email.com" };
            _funcionarioRepositorioMock.Setup(repo => repo.ObterPorIdAsync(1)).ReturnsAsync(funcionario);

            // Act
            var resultado = await _funcionarioServico.ObterPorIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("João", resultado.Nome);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveLancarExcecao_QuandoFuncionarioNaoExiste()
        {
            // Arrange
            _funcionarioRepositorioMock.Setup(repo => repo.ObterPorIdAsync(1)).ReturnsAsync((Funcionario)null);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<Exception>(() => _funcionarioServico.ObterPorIdAsync(1));
            Assert.Equal("Funcionário não encontrado.", excecao.Message);
        }

        [Fact]
        public async Task CriarAsync_DeveCriarFuncionario_QuandoDadosValidos()
        {
            // Arrange
            var criarFuncionarioDto = new CriarFuncionarioDto
            {
                Nome = "Carlos",
                Sobrenome = "Silva",
                Email = "carlos@email.com",
                Documento = "12345678900",
                DataNascimento = DateTime.Today.AddYears(-25),
                Senha = "SenhaForte123",
                Telefones = new List<TelefoneDto> { new TelefoneDto { Numero = "11999999999" } }
            };

            _funcionarioRepositorioMock.Setup(repo => repo.AdicionarAsync(It.IsAny<Funcionario>()))
                                       .Returns(Task.CompletedTask);

            // Act
            var resultado = await _funcionarioServico.CriarAsync(criarFuncionarioDto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Carlos", resultado.Nome);
        }

        [Fact]
        public async Task CriarAsync_DeveLancarExcecao_QuandoIdadeMenorQue18()
        {
            // Arrange
            var criarFuncionarioDto = new CriarFuncionarioDto
            {
                Nome = "Lucas",
                DataNascimento = DateTime.Today.AddYears(-17),
                Senha = "SenhaForte123"
            };

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<Exception>(() => _funcionarioServico.CriarAsync(criarFuncionarioDto));
            Assert.Equal("Funcionário deve ser maior de idade.", excecao.Message);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarFuncionario_QuandoExistir()
        {
            // Arrange
            var funcionario = new Funcionario { Id = 1, Nome = "João", Email = "joao@email.com" };
            var atualizarDto = new AtualizarFuncionarioDto
            {
                Nome = "João Alterado",
                Documento = "12345678900",
                DataNascimento = DateTime.Today.AddYears(-30),
                Telefones = new List<TelefoneDto> { new TelefoneDto { Numero = "11988888888" } }
            };

            _funcionarioRepositorioMock.Setup(repo => repo.ObterPorIdAsync(1)).ReturnsAsync(funcionario);
            _funcionarioRepositorioMock.Setup(repo => repo.AtualizarAsync(funcionario)).Returns(Task.CompletedTask);

            // Act
            await _funcionarioServico.AtualizarAsync(1, atualizarDto);

            // Assert
            _funcionarioRepositorioMock.Verify(repo => repo.AtualizarAsync(It.IsAny<Funcionario>()), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRemoverFuncionario_QuandoExistir()
        {
            // Arrange
            var funcionario = new Funcionario { Id = 1, Nome = "Carlos" };
            _funcionarioRepositorioMock.Setup(repo => repo.ObterPorIdAsync(1)).ReturnsAsync(funcionario);
            _funcionarioRepositorioMock.Setup(repo => repo.RemoverAsync(funcionario)).Returns(Task.CompletedTask);

            // Act
            await _funcionarioServico.RemoverAsync(1);

            // Assert
            _funcionarioRepositorioMock.Verify(repo => repo.RemoverAsync(It.IsAny<Funcionario>()), Times.Once);
        }

        [Fact]
        public async Task AutenticarAsync_DeveRetornarTrue_QuandoSenhaCorreta()
        {
            // Arrange
            string senhaHash = BCrypt.Net.BCrypt.HashPassword("SenhaForte123");
            var funcionario = new Funcionario { Id = 1, Email = "joao@email.com", SenhaHash = senhaHash };

            _funcionarioRepositorioMock.Setup(repo => repo.ObterPorEmailAsync("joao@email.com")).ReturnsAsync(funcionario);

            // Act
            var resultado = await _funcionarioServico.AutenticarAsync("joao@email.com", "SenhaForte123");

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task AutenticarAsync_DeveRetornarFalse_QuandoSenhaIncorreta()
        {
            // Arrange
            string senhaHash = BCrypt.Net.BCrypt.HashPassword("SenhaForte123");
            var funcionario = new Funcionario { Id = 1, Email = "joao@email.com", SenhaHash = senhaHash };

            _funcionarioRepositorioMock.Setup(repo => repo.ObterPorEmailAsync("joao@email.com")).ReturnsAsync(funcionario);

            // Act
            var resultado = await _funcionarioServico.AutenticarAsync("joao@email.com", "SenhaErrada");

            // Assert
            Assert.False(resultado);
        }
    }
}
