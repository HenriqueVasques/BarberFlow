using BarberFlow.API.DTOs.Profissional;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfissionalController : ControllerBase
    {
        private readonly ProfissionalService _profissionalService;

        public ProfissionalController(ProfissionalService profissionalService)
        {
            _profissionalService = profissionalService;
        }

        #region Comandos: Escrita (Admin / Gestão)

        // Cadastra um novo profissional vinculado a uma empresa
        [HttpPost]
        public async Task<IActionResult> CriarProfissional([FromBody] ProfissionalCreateDto dto)
        {
            try
            {
                var profissional = await _profissionalService.CriarProfissional(dto);
                return StatusCode(201, new { message = "Profissional criado com sucesso!", dados = profissional });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Atualiza os dados cadastrais do profissional pelo ID
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProfissional(long id, [FromBody] ProfissionalUpdateDto dto)
        {
            try
            {
                await _profissionalService.AtualizarProfissional(id, dto);
                return Ok(new { message = "Profissional atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Soft Delete (desativação) do profissional
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProfissional(long id)
        {
            try
            {
                await _profissionalService.DeletarProfissional(id);
                return Ok(new { message = "Profissional deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Consultas: Leitura

        // Obtém o perfil completo de um profissional pelo seu ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterProfissionalPorId(long id)
        {
            try
            {
                var profissional = await _profissionalService.ObterPorId(id);
                return Ok(new 
                { 
                    message = "Profissional obtido com sucesso!", 
                    dados = profissional 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Lista todos os profissionais ativos pertencentes a uma empresa específica
        [HttpGet("empresa/{empresaId}")]
        public async Task<IActionResult> ObterProfissionaisPorEmpresa(long empresaId)
        {
            try
            {
                var profissionais = await _profissionalService.ObterProfissionaisPorEmpresa(empresaId);
                return Ok(new 
                { 
                    message = "Profissionais obtidos com sucesso!", 
                    dados = profissionais 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion
    }
}