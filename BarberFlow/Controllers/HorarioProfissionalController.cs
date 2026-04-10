using BarberFlow.API.DTOs;
using BarberFlow.API.Models;
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

        [HttpPost]
        public async Task<IActionResult> CriarHorarioProfissional(HorarioProfissionalCreateDto dto)
        {
            try
            {
                var horarioProfissional = await _horarioProfissionalService.AdicionarHorarioProfissional(dto);
                var response = MapearParaResponseDto(horarioProfissional);
                return StatusCode(201, new
                {
                    message = "Horário do Profissional criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarHorarioProfissional(long id, HorarioProfissionalUpdateDto dto)
        {
            try
            {
                var horarioProfissional = await _horarioProfissionalService.AtualizarHorarioProfissional(id, dto);
                var response = MapearParaResponseDto(horarioProfissional);
                return StatusCode(200, new
                {
                    message = "Horário do Profissional atualizado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarHorarioProfissional(long id)
        {
            try
            {
                var horarioProfissional = await _horarioProfissionalService.DeletarHorarioProfissional(id);
                var response = MapearParaResponseDto(horarioProfissional);
                return StatusCode(200, new
                {
                    message = "Horário do Profissional deletado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterPorId(long id)
        {
            try
            {
                var horarioProfissional = await _horarioProfissionalService.ObterPorId(id);
                var response = MapearParaResponseDto(horarioProfissional);
                return Ok(new
                {
                    message = "Horário do Profissional obtido com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("admin/obter-pelo-profissionalId/{id}")]
        public async Task<IActionResult> ObterPorProfissionalId(long profissionalId)
        {
            try
            {
                var horariosProfissionais = await _horarioProfissionalService.ObterPorProfissionalId(profissionalId);
                return Ok(new
                {
                    message = "Horários dos Profissionais obtidos com sucesso!",
                    dados = horariosProfissionais
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private object MapearParaResponseDto(HorarioProfissional horarioProfissional)
        {
            return new HorarioProfissionalResponseDto
            {
                Id = horarioProfissional.Id,
                ProfissionalId = horarioProfissional.ProfissionalId,
                EmpresaId = horarioProfissional.EmpresaId,
                DiaSemana = horarioProfissional.DiaSemana,
                HoraInicio = horarioProfissional.HoraInicio,
                HoraFim = horarioProfissional.HoraFim,
                HoraInicioAlmoco = horarioProfissional.HoraInicioAlmoco,
                HoraFimAlmoco = horarioProfissional.HoraFimAlmoco,
                Ativo = horarioProfissional.Ativo
            };
        }
    }
}
