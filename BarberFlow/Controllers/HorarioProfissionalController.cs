using BarberFlow.API.DTOs;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioProfissionalController : ControllerBase
    {
        private readonly HorarioProfissionalService _horarioProfissionalService;

        public HorarioProfissionalController(HorarioProfissionalService horarioProfissionalService)
        {
            _horarioProfissionalService = horarioProfissionalService;
        }

        #region Comandos: Escrita (Admin)

        // Cria uma nova configuração de horário para um determinado profissional
        [HttpPost]
        public async Task<IActionResult> CriarHorarioProfissional([FromBody] HorarioProfissionalCreateDto dto)
        {
            try
            {
                var horarioProfissional = await _horarioProfissionalService.CriarHorarioProfissional(dto);
                return StatusCode(201, new { message = "Horário do Profissional criado com sucesso!", dados = horarioProfissional });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Atualiza os dados de um horário de trabalho existente de um profissional
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarHorarioProfissional(long id, [FromBody] HorarioProfissionalUpdateDto dto)
        {
            try
            {
                await _horarioProfissionalService.AtualizarHorarioProfissional(id, dto);
                return Ok(new { message = "Horário do Profissional atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Remove (soft delete) uma configuração de horário do profissional
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarHorarioProfissional(long id)
        {
            try
            {
                await _horarioProfissionalService.DeletarHorarioProfissional(id);
                return Ok(new { message = "Horário do Profissional removido com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Consultas: Leitura

        // Obtém os detalhes de um horário específico do profissional pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            try
            {
                var horarioProfissional = await _horarioProfissionalService.ObterPorId(id);
                return Ok(new { message = "Horário recuperado com sucesso!", dados = horarioProfissional });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Lista a grade completa de horários cadastrados para um profissional específico
        [HttpGet("profissional/{profissionalId}")]
        public async Task<IActionResult> ObterPorProfissionalId(long profissionalId)
        {
            try
            {
                var horarios = await _horarioProfissionalService.ObterPorProfissionalId(profissionalId);
                return Ok(new { message = "Agenda do profissional recuperada com sucesso!", dados = horarios });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion
    }
}