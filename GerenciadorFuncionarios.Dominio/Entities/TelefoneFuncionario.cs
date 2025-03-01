using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFuncionarios.Dominio.Entities
{
    public class TelefoneFuncionario
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int FuncionarioId { get; set; }
        public virtual Funcionario? Funcionario { get; set; }
    }
}
