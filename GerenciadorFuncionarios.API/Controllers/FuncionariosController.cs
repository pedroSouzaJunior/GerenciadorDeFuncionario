using GerenciadorFuncionarios.Aplicacao.DTOs;
using GerenciadorFuncionarios.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace GerenciadorFuncionarios.API.Controllers
{
    [ApiController]
    [Route("api/funcionarios")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
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
        /// Obtém uma lista de todos os funcionários cadastrados.
        /// </summary>
        /// <remarks>
        /// Somente usuários com as permissões "Admin" ou "Usuario" podem acessar este endpoint.
        /// </remarks>
        /// <returns>Uma lista de funcionários.</returns>
        /// <response code="200">Retorna a lista de funcionários cadastrados.</response>
        /// <response code="401">O usuário não está autenticado.</response>
        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        [ProducesResponseType(typeof(IEnumerable<FuncionarioDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<FuncionarioDto>>> ObterTodos()
        {
            _logger.LogInformation("Solicitação para obter todos os funcionários recebida.");

            var funcionarios = await _funcionarioServico.ObterTodosAsync();

            _logger.LogInformation("Retornando {Quantidade} funcionários.", funcionarios.Count());
            return Ok(funcionarios);
        }

        /// <summary>
        /// Obtém um funcionário específico pelo ID.
        /// </summary>
        /// <remarks>
        /// Somente usuários com as permissões "Admin" ou "Usuario" podem acessar este endpoint.
        /// </remarks>
        /// <param name="id">O ID do funcionário a ser recuperado.</param>
        /// <returns>Os detalhes do funcionário.</returns>
        /// <response code="200">Retorna os dados do funcionário.</response>
        /// <response code="404">Funcionário não encontrado.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Usuario")]
        [ProducesResponseType(typeof(FuncionarioDto), 200)]
        [ProducesResponseType(404)]
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
        /// Cria um novo funcionário no sistema.
        /// </summary>
        /// <remarks>
        /// Somente usuários com as permissões "Admin" ou "Usuario" podem acessar este endpoint.
        /// Usuários não podem criar contas com um nível de permissão maior que o seu.
        /// </remarks>
        /// <param name="criarFuncionarioDto">Os dados do novo funcionário.</param>
        /// <returns>Os dados do funcionário criado.</returns>
        /// <response code="201">Funcionário criado com sucesso.</response>
        /// <response code="400">Os dados fornecidos são inválidos.</response>
        /// <response code="403">Tentativa de criar um usuário com um nível de permissão superior.</response>
        [HttpPost]
        [Authorize(Roles = "Admin,Usuario")]
        [ProducesResponseType(typeof(FuncionarioDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
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
        /// Atualiza os dados de um funcionário existente.
        /// </summary>
        /// <remarks>
        /// Somente usuários com as permissões "Admin" ou "Usuario" podem acessar este endpoint.
        /// Não é permitido alterar a senha por este endpoint.
        /// </remarks>
        /// <param name="id">O ID do funcionário a ser atualizado.</param>
        /// <param name="funcionarioDto">Os novos dados do funcionário.</param>
        /// <response code="204">Funcionário atualizado com sucesso.</response>
        /// <response code="404">Funcionário não encontrado.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Usuario")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarFuncionarioDto funcionarioDto)
        {
            _logger.LogInformation("Solicitação para atualizar funcionário com ID {FuncionarioId}.", id);

            await _funcionarioServico.AtualizarAsync(id, funcionarioDto);

            _logger.LogInformation("Funcionário com ID {FuncionarioId} atualizado com sucesso.", id);
            return NoContent();
        }

        /// <summary>
        /// Remove um funcionário do sistema.
        /// </summary>
        /// <remarks>
        /// Apenas usuários com a permissão "Admin" podem remover funcionários.
        /// </remarks>
        /// <param name="id">O ID do funcionário a ser removido.</param>
        /// <response code="204">Funcionário removido com sucesso.</response>
        /// <response code="403">Usuário não tem permissão para remover funcionários.</response>
        /// <response code="404">Funcionário não encontrado.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Remover(int id)
        {
            _logger.LogInformation("Solicitação para remover funcionário com ID {FuncionarioId}.", id);

            await _funcionarioServico.RemoverAsync(id);

            _logger.LogInformation("Funcionário com ID {FuncionarioId} removido com sucesso.", id);
            return NoContent();
        }
    }
}
