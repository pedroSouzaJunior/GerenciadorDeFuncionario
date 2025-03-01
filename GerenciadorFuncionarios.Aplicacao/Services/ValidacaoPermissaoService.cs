using GerenciadorFuncionarios.Aplicacao.Interfaces;
using System.Security.Claims;

namespace GerenciadorFuncionarios.Aplicacao.Services
{
    public class ValidacaoPermissaoService : IValidacaoPermissaoService
    {
        public void ValidarCriacaoDeFuncionario(ClaimsPrincipal usuarioAutenticado, string roleNova)
        {
            var roleUsuario = usuarioAutenticado.FindFirst("role")?.Value ?? "Usuario";

            if (roleUsuario == "Usuario" && roleNova == "Admin")
            {
                throw new UnauthorizedAccessException("Você não tem permissão para criar usuários administradores.");
            }
        }
    }
}
