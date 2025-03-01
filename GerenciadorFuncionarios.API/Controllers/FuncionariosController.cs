using GerenciadorFuncionarios.Aplicacao.DTOs;
using GerenciadorFuncionarios.Aplicacao.Services;
using GerenciadorFuncionarios.Dominio.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorFuncionarios.API.Controllers
{
    [ApiController]
    [Route("api/funcionarios")]
    [Authorize]
    public class FuncionariosController : ControllerBase
    {
        private readonly FuncionarioServico _funcionarioServico;

        public FuncionariosController(FuncionarioServico funcionarioServico)
        {
            _funcionarioServico = funcionarioServico;
        }

        /// <summary>
        /// Obtém todos os funcionários.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioDto>>> ObterTodos()
        {
            var funcionarios = await _funcionarioServico.ObterTodosAsync();
            return Ok(funcionarios);
        }

        /// <summary>
        /// Obtém um funcionário pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioDto>> ObterPorId(int id)
        {
            var funcionario = await _funcionarioServico.ObterPorIdAsync(id);
            if (funcionario == null)
                return NotFound(new { mensagem = "Funcionário não encontrado." });

            return Ok(funcionario);
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FuncionarioDto>> Criar([FromBody] CriarFuncionarioDto criarFuncionarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var novoFuncionario = await _funcionarioServico.CriarAsync(criarFuncionarioDto);
            return CreatedAtAction(nameof(ObterPorId), new { id = novoFuncionario.Id }, novoFuncionario);
        }

        /// <summary>
        /// Atualiza um funcionário existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CriarFuncionarioDto funcionarioDto)
        {
            await _funcionarioServico.AtualizarAsync(id, funcionarioDto);
            return NoContent();
        }

        /// <summary>
        /// Remove um funcionário pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            await _funcionarioServico.RemoverAsync(id);
            return NoContent();
        }
    }
}
