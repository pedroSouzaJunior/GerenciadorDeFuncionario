using GerenciadorFuncionarios.Aplicacao.DTOs;

namespace GerenciadorFuncionarios.Aplicacao.Interfaces
{
    public interface IFuncionarioServico
    {
        Task<IEnumerable<FuncionarioDto>> ObterTodosAsync();
        Task<FuncionarioDto> ObterPorIdAsync(int id);
        Task<FuncionarioDto> CriarAsync(CriarFuncionarioDto criarFuncionarioDto);
        Task AtualizarAsync(int id, AtualizarFuncionarioDto funcionarioDto);
        Task RemoverAsync(int id);
        Task<bool> AutenticarAsync(string email, string senha);
        Task<FuncionarioLoginDto> ObterPorEmailAsync(string email);
    }
}
