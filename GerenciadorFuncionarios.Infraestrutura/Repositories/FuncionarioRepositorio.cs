using GerenciadorFuncionarios.Aplicacao.Interfaces;
using GerenciadorFuncionarios.Dominio.Entities;
using GerenciadorFuncionarios.Infraestrutura.Dados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFuncionarios.Infraestrutura.Repositories
{
    public class FuncionarioRepositorio : IFuncionarioRepositorio
    {
        private readonly AppDbContext _context;

        public FuncionarioRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Funcionario>> ObterTodosAsync()
        {
            return await _context.Funcionarios.Include(f => f.Telefones).ToListAsync();
        }

        public async Task<Funcionario> ObterPorIdAsync(int id)
        {
            return await _context.Funcionarios.Include(f => f.Telefones)
                                              .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AdicionarAsync(Funcionario funcionario)
        {
            await _context.Funcionarios.AddAsync(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Funcionarios.AnyAsync(f => f.Id == id);
        }

        public async Task<Funcionario?> ObterPorEmailAsync(string email)
        {
            return await _context.Funcionarios
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Email == email);
        }
    }
}
