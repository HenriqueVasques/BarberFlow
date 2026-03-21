using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.Enums;
using BarberFlow.API.Models;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoController : ControllerBase
    {
        private readonly AgendamentoService _agendamentoService;

        public AgendamentoController(AgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }

        #region Endpoints de Escrita

        [HttpPost]
        public async Task<IActionResult> CriarAgendamento(AgendamentoCreateDto dto)
        {
            try
            {
                var agendamento = await _agendamentoService.AdicionarAgendamento(dto);
                var response = MapearParaResponseDto(agendamento);

                return CreatedAtAction(nameof(ObterPorId), new { id = agendamento.Id }, new
                {
                    message = "Agendamento criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> CancelarAgendamento(long id)
        {
            try
            {
                var agendamento = await _agendamentoService.Cancelar(id);
                var response = MapearParaResponseDto(agendamento);

                return Ok(new
                {
                    message = "Agendamento cancelado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Endpoints de Leitura

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            try
            {
                var agendamento = await _agendamentoService.ObterPorId(id);
                var response = MapearParaResponseDto(agendamento);

                return Ok(new
                {
                    message = "Agendamento obtido com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("agenda")]
        public async Task<IActionResult> ObterAgenda( [FromQuery] long profissionalId, [FromQuery] long empresaId, [FromQuery] DateTime inicio, [FromQuery] DateTime fim, [FromQuery] List<StatusAgendamento> status)
        {
            try
            {
                if (status == null || !status.Any())
                {
                    status = new List<StatusAgendamento> { StatusAgendamento.Pendente, StatusAgendamento.Confirmado };
                }

                var agenda = await _agendamentoService.ObterAgendaPorPeriodo(profissionalId, empresaId, inicio, fim, status);

                var response = agenda.Select(a => new AgendamentoAgendaDiaDto
                {
                    NomeCliente = a.Cliente?.Usuario?.Nome ?? "N/A",
                    NomeServico = a.Servico?.Nome ?? "N/A",
                    InicioDoDia = a.DataHoraInicio,
                    FimDoDia = a.DataHoraFim,
                    Status = a.Status,
                    Preco = a.PrecoNoMomento
                }).ToList();

                return Ok(new
                {
                    message = "Agenda obtida com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Métodos Auxiliares (Private)

        /// Centraliza o mapeamento para evitar repetição de código (DRY - Don't Repeat Yourself)
        private static AgendamentoResponseDto MapearParaResponseDto(Agendamento agendamento)
        {
            return new AgendamentoResponseDto
            {
                Id = agendamento.Id,
                NomeProfissional = agendamento.Profissional?.Usuario?.Nome ?? "N/A",
                NomeCliente = agendamento.Cliente?.Usuario?.Nome ?? "N/A",
                NomeServico = agendamento.Servico?.Nome ?? "N/A",
                NomeEmpresa = agendamento.Empresa?.Nome ?? "N/A",
                EmpresaId = agendamento.EmpresaId,
                ProfissionalId = agendamento.ProfissionalId,
                ClienteId = agendamento.ClienteId,
                ServicoId = agendamento.ServicoId,
                PrecoNoMomento = agendamento.PrecoNoMomento,
                DataHoraInicio = agendamento.DataHoraInicio,
                DataHoraFim = agendamento.DataHoraFim,
                Status = agendamento.Status,
                DataCriacao = agendamento.DataCriacao,
                DataAtualizacao = agendamento.DataAtualizacao
            };
        }

        #endregion
    }
}