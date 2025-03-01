using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorFuncionarios.Aplicacao.DTOs
{
    public class AtualizarFuncionarioDto
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Sobrenome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Sobrenome deve ter no máximo 100 caracteres.")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "O campo Documento é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O Documento deve conter 11 números.")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public int? GestorId { get; set; }

        public List<TelefoneDto> Telefones { get; set; } = new();
    }
}
