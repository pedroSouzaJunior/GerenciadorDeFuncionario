using GerenciadorFuncionarios.Aplicacao.DTOs;
using GerenciadorFuncionarios.Aplicacao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly FuncionarioServico _funcionarioServico;
    private readonly TokenService _tokenService;

    public AuthController(FuncionarioServico funcionarioServico, TokenService tokenService)
    {
        _funcionarioServico = funcionarioServico;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var funcionario = await _funcionarioServico.ObterPorEmailAsync(loginDto.Email);
        if (funcionario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, funcionario.SenhaHash))
        {
            return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });
        }

        var funcionarioDto = new FuncionarioDto
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome,
            Email = funcionario.Email
        };

        var token = _tokenService.GenerateToken(funcionarioDto);
        return Ok(new { token });
    }
}
