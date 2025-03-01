using FluentValidation;
using GerenciadorFuncionarios.Aplicacao.DTOs;

public class CriarFuncionarioValidator : AbstractValidator<CriarFuncionarioDto>
{
    public CriarFuncionarioValidator()
    {
        RuleFor(f => f.Nome)
            .NotEmpty().WithMessage("O campo Nome é obrigatório.")
            .MaximumLength(100).WithMessage("O Nome deve ter no máximo 100 caracteres.");

        RuleFor(f => f.Sobrenome)
            .NotEmpty().WithMessage("O campo Sobrenome é obrigatório.")
            .MaximumLength(100).WithMessage("O Sobrenome deve ter no máximo 100 caracteres.");

        RuleFor(f => f.Email)
            .NotEmpty().WithMessage("O campo E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(f => f.Documento)
            .NotEmpty().WithMessage("O campo Documento é obrigatório.")
            .Matches(@"^\d{11}$").WithMessage("O Documento deve conter 11 números.");

        RuleFor(f => f.DataNascimento)
            .NotEmpty().WithMessage("O campo Data de Nascimento é obrigatório.")
            .Must(data => (DateTime.Today.Year - data.Year) >= 18)
            .WithMessage("Funcionário deve ser maior de idade.");

        RuleFor(f => f.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");
    }
}
