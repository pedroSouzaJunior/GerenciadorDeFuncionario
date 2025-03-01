using System.Security.Claims;

namespace GerenciadorFuncionarios.Aplicacao.Interfaces
{
    public interface IValidacaoPermissaoService
    {
        void ValidarCriacaoDeFuncionario(ClaimsPrincipal usuarioAutenticado, string roleNova);
    }
}
