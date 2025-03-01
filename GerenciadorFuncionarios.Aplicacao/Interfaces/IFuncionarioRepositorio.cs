using GerenciadorFuncionarios.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFuncionarios.Aplicacao.Interfaces
{
    public interface IFuncionarioRepositorio
    {
        Task<IEnumerable<Funcionario>> ObterTodosAsync();
        Task<Funcionario> ObterPorIdAsync(int id);
        Task AdicionarAsync(Funcionario funcionario);
        Task AtualizarAsync(Funcionario funcionario);
        Task RemoverAsync(Funcionario funcionario);
        Task<bool> ExisteAsync(int id);
        Task<Funcionario?> ObterPorEmailAsync(string email);
    }
}
