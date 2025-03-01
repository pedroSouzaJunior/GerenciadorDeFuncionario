using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFuncionarios.Aplicacao.DTOs
{
    public class FuncionarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Sobrenome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Documento { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public int? GestorId { get; set; }
        public List<TelefoneDto> Telefones { get; set; } = new();
    }
}
