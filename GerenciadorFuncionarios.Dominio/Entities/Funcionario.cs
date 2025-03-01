using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFuncionarios.Dominio.Entities
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }
        public DateTime DataNascimento { get; set; }
        public virtual List<TelefoneFuncionario> Telefones { get; set; } = new List<TelefoneFuncionario>();
        public int? GestorId { get; set; }
        public virtual Funcionario? Gestor { get; set; }
        public string SenhaHash { get; set; } = null!;
    }
}
