using GerenciadorFuncionarios.Aplicacao.DTOs;
using GerenciadorFuncionarios.Aplicacao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly FuncionarioServico _funcionarioServico;
    private readonly TokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(FuncionarioServico funcionarioServico, TokenService tokenService, ILogger<AuthController> logger)
    {
        _funcionarioServico = funcionarioServico;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation("Solicitação de login recebida para o usuário {Email}.", loginDto.Email);

        var funcionario = await _funcionarioServico.ObterPorEmailAsync(loginDto.Email);
        if (funcionario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, funcionario.SenhaHash))
        {
            _logger.LogWarning("Falha no login para o usuário {Email}: credenciais inválidas.", loginDto.Email);
            return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });
        }

        var funcionarioDto = new FuncionarioDto
        {
            Id = funcionario.Id,
            Nome = funcionario.Nome,
            Email = funcionario.Email,
            Role = funcionario.Role
        };

        var token = _tokenService.GenerateToken(funcionarioDto);

        _logger.LogInformation("Login bem-sucedido para o usuário {Email}.", loginDto.Email);
        return Ok(new { token });
    }
}
