using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.DTOs.BloqueioHorario;
using BarberFlow.API.Models;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloqueioHorarioController : ControllerBase
    {
        private readonly BloqueioHorarioService _bloqueioHorarioService;

        public BloqueioHorarioController(BloqueioHorarioService bloqueioHorarioService)
        {
            _bloqueioHorarioService = bloqueioHorarioService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarBloqueioHorario([FromBody] BloqueioHorarioCreateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }

                var bloqueioHorario = await _bloqueioHorarioService.CriarBloqueioHorario(dto);

                var response = MapearParaResponseDto(bloqueioHorario);
                return StatusCode(201, new
                {
                    message = "Bloqueio de horário criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarBloqueioHorario(long id, [FromBody] BloqueioHorarioUpdateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }
                var bloqueioHorario = await _bloqueioHorarioService.AtualizarBloqueioHorario(id, dto);
                var response = MapearParaResponseDto(bloqueioHorario);
                return Ok(new
                {
                    message = "Bloqueio de horário atualizado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
        [HttpDelete]
        public async Task<IActionResult> DeletarBloqueioHorario(long id)
        {
            try
            {
                await _bloqueioHorarioService.DeletarBloqueioHorario(id);
                return Ok(new { message = "Bloqueio de horário deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpGet("{empresaId}/por-empresa")]
        public async Task<IActionResult> ObterPorEmpresaId(long empresaId)
        {
            try
            {
                var bloqueioHorario = await _bloqueioHorarioService.ObterPorEmpresaId(empresaId);
                var response = bloqueioHorario.Select(b => new BloqueioHorarioResponseDto
                {
                    Id = b.Id,
                    NomeProfissional = b.Profissional?.Usuario?.Nome ?? "N/A",
                    NomeEmpresa = b.Empresa?.Nome ?? "N/A",
                    EmpresaId = b.EmpresaId,
                    ProfissionalId = b.ProfissionalId,
                    DataHoraInicio = b.DataHoraInicio,
                    DataHoraFim = b.DataHoraFim,
                    DataCriacao = b.DataCriacao,
                    DataAtualizacao = b.DataAtualizacao,
                    Motivo = b.Motivo
                });
                return Ok(new
                {
                    message = "Bloqueio de horário obtido com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
        [HttpGet("{profissionalId}/por-profissional")]
        public async Task<IActionResult> ObterPorProfissionalId(long profissionalId)
        {
            try
            {
                var bloqueioHorario = await _bloqueioHorarioService.ObterPorProfissionalId(profissionalId);
                var response = bloqueioHorario.Select(b => new BloqueioHorarioResponseDto
                {
                    Id = b.Id,
                    NomeProfissional = b.Profissional?.Usuario?.Nome ?? "N/A",
                    NomeEmpresa = b.Empresa?.Nome ?? "N/A",
                    EmpresaId = b.EmpresaId,
                    ProfissionalId = b.ProfissionalId,
                    DataHoraInicio = b.DataHoraInicio,
                    DataHoraFim = b.DataHoraFim,
                    DataCriacao = b.DataCriacao,
                    DataAtualizacao = b.DataAtualizacao,
                    Motivo = b.Motivo
                });
                return Ok(new
                {
                    message = "Bloqueio de horário obtido com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        private static BloqueioHorarioResponseDto MapearParaResponseDto(BloqueioHorario bloqueio)
        {
            return new BloqueioHorarioResponseDto
            {
                Id = bloqueio.Id,
                NomeProfissional = bloqueio.Profissional?.Usuario?.Nome ?? "N/A",
                NomeEmpresa = bloqueio.Empresa?.Nome ?? "N/A",
                EmpresaId = bloqueio.EmpresaId,
                ProfissionalId = bloqueio.ProfissionalId,
                DataHoraInicio = bloqueio.DataHoraInicio,
                DataHoraFim = bloqueio.DataHoraFim,
                DataCriacao = bloqueio.DataCriacao,
                DataAtualizacao = bloqueio.DataAtualizacao,
                Motivo = bloqueio.Motivo
            };
        }
    }
}
