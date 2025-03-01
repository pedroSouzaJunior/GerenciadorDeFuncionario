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

    /// <summary>
    /// Construtor do AuthController.
    /// </summary>
    /// <param name="funcionarioServico">Serviço de gerenciamento de funcionários.</param>
    /// <param name="tokenService">Serviço de geração de token JWT.</param>
    /// <param name="logger">Serviço de logging.</param>
    public AuthController(FuncionarioServico funcionarioServico, TokenService tokenService, ILogger<AuthController> logger)
    {
        _funcionarioServico = funcionarioServico;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Realiza o login de um usuário e retorna um token JWT para autenticação.
    /// </summary>
    /// <param name="loginDto">Objeto contendo e-mail e senha do usuário.</param>
    /// <returns>
    /// - **200 OK**: Retorna o token JWT caso as credenciais sejam válidas.<br/>
    /// - **401 Unauthorized**: Caso o e-mail ou senha estejam incorretos.<br/>
    /// - **400 Bad Request**: Caso o formato da requisição seja inválido.
    /// </returns>
    /// <response code="200">Login bem-sucedido, retorna o token JWT.</response>
    /// <response code="401">Usuário ou senha inválidos.</response>
    /// <response code="400">Formato inválido da requisição.</response>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation("Solicitação de login recebida para o usuário {Email}.", loginDto.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Tentativa de login falhou devido a formato inválido.");
            return BadRequest(ModelState);
        }

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
