using System.Security.Claims;
using GerenciadorFuncionarios.Aplicacao.Services;
using Xunit;

namespace GerenciadorFuncionarios.Testes
{
    public class ValidacaoPermissaoServiceTests
    {
        private readonly ValidacaoPermissaoService _validacaoPermissaoService;

        public ValidacaoPermissaoServiceTests()
        {
            _validacaoPermissaoService = new ValidacaoPermissaoService();
        }

        [Fact]
        public void UsuarioNaoPodeCriarAdmin_DeveLancarExcecao()
        {
            // Arrange
            var claims = new List<Claim> { new Claim("role", "Usuario") };
            var identity = new ClaimsIdentity(claims);
            var usuario = new ClaimsPrincipal(identity);

            // Act & Assert
            var exception = Assert.Throws<UnauthorizedAccessException>(() =>
                _validacaoPermissaoService.ValidarCriacaoDeFuncionario(usuario, "Admin"));

            Assert.Equal("Você não tem permissão para criar usuários administradores.", exception.Message);
        }

        [Fact]
        public void AdminPodeCriarUsuario_NaoDeveLancarExcecao()
        {
            // Arrange
            var claims = new List<Claim> { new Claim("role", "Admin") };
            var identity = new ClaimsIdentity(claims);
            var usuario = new ClaimsPrincipal(identity);

            // Act & Assert
            _validacaoPermissaoService.ValidarCriacaoDeFuncionario(usuario, "Usuario");
        }
    }
}
