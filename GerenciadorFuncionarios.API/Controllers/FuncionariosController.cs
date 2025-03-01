using GerenciadorFuncionarios.Aplicacao.DTOs;
using GerenciadorFuncionarios.Aplicacao.Interfaces;
using GerenciadorFuncionarios.Aplicacao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GerenciadorFuncionarios.API.Controllers
{
    [ApiController]
    [Route("api/funcionarios")]
    [Authorize]
    public class FuncionariosController : ControllerBase
    {
        private readonly IFuncionarioServico _funcionarioServico;
        private readonly ILogger<FuncionariosController> _logger;
        private readonly IValidacaoPermissaoService _validacaoPermissaoService;

        public FuncionariosController(
            IFuncionarioServico funcionarioServico,
            IValidacaoPermissaoService validacaoPermissaoService,
            ILogger<FuncionariosController> logger)
        {
            _funcionarioServico = funcionarioServico;
            _validacaoPermissaoService = validacaoPermissaoService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os funcionários.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        public async Task<ActionResult<IEnumerable<FuncionarioDto>>> ObterTodos()
        {
            _logger.LogInformation("Solicitação para obter todos os funcionários recebida.");

            var funcionarios = await _funcionarioServico.ObterTodosAsync();

            _logger.LogInformation("Retornando {Quantidade} funcionários.", funcionarios.Count());
            return Ok(funcionarios);
        }

        /// <summary>
        /// Obtém um funcionário pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Usuario")]
        public async Task<ActionResult<FuncionarioDto>> ObterPorId(int id)
        {
            _logger.LogInformation("Solicitação para obter funcionário com ID {FuncionarioId}.", id);

            var funcionario = await _funcionarioServico.ObterPorIdAsync(id);
            if (funcionario == null)
            {
                _logger.LogWarning("Funcionário com ID {FuncionarioId} não encontrado.", id);
                return NotFound(new { mensagem = "Funcionário não encontrado." });
            }

            _logger.LogInformation("Funcionário com ID {FuncionarioId} encontrado.", id);
            return Ok(funcionario);
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Usuario")]
        public async Task<ActionResult<FuncionarioDto>> Criar([FromBody] CriarFuncionarioDto criarFuncionarioDto)
        {
            _logger.LogInformation("Solicitação para criar um novo funcionário: {Nome}, {Email}.", criarFuncionarioDto.Nome, criarFuncionarioDto.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos para criação do funcionário.");
                return BadRequest(ModelState);
            }

            try
            {
                _validacaoPermissaoService.ValidarCriacaoDeFuncionario(User, criarFuncionarioDto.Role);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Usuário {Email} tentou criar um administrador sem permissão.", User.Identity.Name);
                return Forbid("Você não tem permissão para criar usuários administradores.");
            }

            var novoFuncionario = await _funcionarioServico.CriarAsync(criarFuncionarioDto);

            _logger.LogInformation("Funcionário criado com sucesso: ID {FuncionarioId}.", novoFuncionario.Id);
            return CreatedAtAction(nameof(ObterPorId), new { id = novoFuncionario.Id }, novoFuncionario);
        }

        /// <summary>
        /// Atualiza um funcionário existente.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Usuario")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarFuncionarioDto funcionarioDto)
        {
            _logger.LogInformation("Solicitação para atualizar funcionário com ID {FuncionarioId}.", id);

            await _funcionarioServico.AtualizarAsync(id, funcionarioDto);

            _logger.LogInformation("Funcionário com ID {FuncionarioId} atualizado com sucesso.", id);
            return NoContent();
        }

        /// <summary>
        /// Remove um funcionário pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remover(int id)
        {
            _logger.LogInformation("Solicitação para remover funcionário com ID {FuncionarioId}.", id);

            await _funcionarioServico.RemoverAsync(id);

            _logger.LogInformation("Funcionário com ID {FuncionarioId} removido com sucesso.", id);
            return NoContent();
        }
    }
}
