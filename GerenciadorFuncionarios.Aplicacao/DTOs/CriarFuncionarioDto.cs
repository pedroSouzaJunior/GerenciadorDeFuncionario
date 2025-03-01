using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFuncionarios.Aplicacao.DTOs
{
    public class CriarFuncionarioDto
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Sobrenome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Sobrenome deve ter no máximo 100 caracteres.")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Documento é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O Documento deve conter 11 números.")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string Senha { get; set; }

        public int? GestorId { get; set; }

        public List<TelefoneDto> Telefones { get; set; } = new();
    }
}
