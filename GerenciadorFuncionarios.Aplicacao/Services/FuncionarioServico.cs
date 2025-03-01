using GerenciadorFuncionarios.Aplicacao.DTOs;
using GerenciadorFuncionarios.Aplicacao.Interfaces;
using GerenciadorFuncionarios.Dominio.Entities;

namespace GerenciadorFuncionarios.Aplicacao.Services
{
    public class FuncionarioServico : IFuncionarioServico
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public FuncionarioServico(IFuncionarioRepositorio funcionarioRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public async Task<IEnumerable<FuncionarioDto>> ObterTodosAsync()
        {
            var funcionarios = await _funcionarioRepositorio.ObterTodosAsync();

            return funcionarios.Select(f => new FuncionarioDto
            {
                Id = f.Id,
                Nome = f.Nome,
                Sobrenome = f.Sobrenome,
                Email = f.Email,
                Documento = f.Documento,
                DataNascimento = f.DataNascimento,
                GestorId = f.GestorId,
                Telefones = f.Telefones.Select(t => new TelefoneDto
                {
                    Id = t.Id,
                    Numero = t.Numero
                }).ToList()
            });
        }

        public async Task<FuncionarioDto> ObterPorIdAsync(int id)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(id);
            if (funcionario == null) throw new Exception("Funcionário não encontrado.");

            return new FuncionarioDto
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Sobrenome = funcionario.Sobrenome,
                Email = funcionario.Email,
                Documento = funcionario.Documento,
                DataNascimento = funcionario.DataNascimento,
                GestorId = funcionario.GestorId,
                Telefones = funcionario.Telefones.Select(t => new TelefoneDto
                {
                    Id = t.Id,
                    Numero = t.Numero
                }).ToList()
            };
        }

        public async Task<FuncionarioDto> CriarAsync(CriarFuncionarioDto criarFuncionarioDto)
        {
            if ((DateTime.Today.Year - criarFuncionarioDto.DataNascimento.Year) < 18)
                throw new Exception("Funcionário deve ser maior de idade.");

            string senhaHash = BCrypt.Net.BCrypt.HashPassword(criarFuncionarioDto.Senha);

            var funcionario = new Funcionario
            {
                Nome = criarFuncionarioDto.Nome,
                Sobrenome = criarFuncionarioDto.Sobrenome,
                Email = criarFuncionarioDto.Email,
                Documento = criarFuncionarioDto.Documento,
                DataNascimento = criarFuncionarioDto.DataNascimento,
                GestorId = criarFuncionarioDto.GestorId,
                SenhaHash = senhaHash,
                Role = criarFuncionarioDto.Role ?? "Usuario",
                Telefones = criarFuncionarioDto.Telefones.Select(t => new TelefoneFuncionario
                {
                    Numero = t.Numero
                }).ToList()
            };

            await _funcionarioRepositorio.AdicionarAsync(funcionario);

            return new FuncionarioDto
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Email = funcionario.Email,
                Role = funcionario.Role
            };
        }



        public async Task AtualizarAsync(int id, AtualizarFuncionarioDto funcionarioDto)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(id);
            if (funcionario == null)
                throw new Exception("Funcionário não encontrado.");

            funcionario.Nome = funcionarioDto.Nome;
            funcionario.Sobrenome = funcionarioDto.Sobrenome;
            funcionario.Documento = funcionarioDto.Documento;
            funcionario.DataNascimento = funcionarioDto.DataNascimento;
            funcionario.GestorId = funcionarioDto.GestorId;
            funcionario.Telefones = funcionarioDto.Telefones.Select(t => new TelefoneFuncionario
            {
                Numero = t.Numero
            }).ToList();

            await _funcionarioRepositorio.AtualizarAsync(funcionario);
        }

        public async Task RemoverAsync(int id)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorIdAsync(id);
            if (funcionario == null)
                throw new Exception("Funcionário não encontrado.");

            await _funcionarioRepositorio.RemoverAsync(funcionario);
        }

        public async Task<bool> AutenticarAsync(string email, string senha)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorEmailAsync(email);
            if (funcionario == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(senha, funcionario.SenhaHash);
        }

        public async Task<FuncionarioLoginDto> ObterPorEmailAsync(string email)
        {
            var funcionario = await _funcionarioRepositorio.ObterPorEmailAsync(email);
            if (funcionario == null) return null;

            return new FuncionarioLoginDto
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                Email = funcionario.Email,
                SenhaHash = funcionario.SenhaHash,
                Role = funcionario.Role
            };
        }
    }
}
