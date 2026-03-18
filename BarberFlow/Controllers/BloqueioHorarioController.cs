using BarberFlow.API.Data.Context;
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

                var response = new BloqueioHorarioResponseDto
                {
                    Id = bloqueioHorario.Id,
                    NomeProfissional = bloqueioHorario.Profissional.Usuario.Nome,
                    NomeEmpresa = bloqueioHorario.Empresa.Nome,
                    EmpresaId = bloqueioHorario.EmpresaId,
                    ProfissionalId = bloqueioHorario.ProfissionalId,
                    DataHoraInicio = bloqueioHorario.DataHoraInicio,
                    DataHoraFim = bloqueioHorario.DataHoraFim,
                    DataCriacao = bloqueioHorario.DataCriacao,
                    DataAtualizacao = bloqueioHorario.DataAtualizacao,
                    Motivo = bloqueioHorario.Motivo,

                };
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
                var response = new BloqueioHorarioResponseDto
                {
                    Id = bloqueioHorario.Id,
                    NomeProfissional = bloqueioHorario.Profissional?.Usuario?.Nome ?? "Profissional não identificado",
                    NomeEmpresa = bloqueioHorario.Empresa?.Nome ?? "Empresa não identificada",
                    EmpresaId = bloqueioHorario.EmpresaId,
                    ProfissionalId = bloqueioHorario.ProfissionalId,
                    DataHoraInicio = bloqueioHorario.DataHoraInicio,
                    DataHoraFim = bloqueioHorario.DataHoraFim,
                    DataCriacao = bloqueioHorario.DataCriacao,
                    DataAtualizacao = bloqueioHorario.DataAtualizacao,
                    Motivo = bloqueioHorario.Motivo,
                };
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
    }
}
