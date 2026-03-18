using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.Models;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<IActionResult> CriarAgendamento(AgendamentoCreateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }

                var agendamento = await _agendamentoService.AdicionarAgendamento(dto);
                if (agendamento == null)
                {
                    return BadRequest("Não foi possível criar o agendamento.");
                }
                
                var response = new AgendamentoResponseDto
                {
                    Id = agendamento.Id,
                    NomeProfissional = agendamento.Profissional.Usuario.Nome,
                    NomeCliente = agendamento.Cliente.Nome,
                    NomeServico = agendamento.Servico.Nome,
                    NomeEmpresa = agendamento.Empresa.Nome,
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

                return StatusCode(201, new
                {
                    message = "Agendamento criado com sucesso!",
                    dados = response
                });

            }
            catch (Exception ex)
            {
                // Isso vai te mostrar o erro real do Postgres (ex: violação de FK, campo nulo, etc)
                var mensagemReal = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = mensagemReal });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> CancelarAgendamento(long id)
        {
            try
            {
                await _agendamentoService.Cancelar(id);
                return Ok(new { message = "Agendamento cancelado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterAgendaProfissionalPorData(long profissionalId, long empresaId, DateTime data)
        {
            try
            {
                var agenda = await _agendamentoService.ObterAgendaProfissionalPorData(profissionalId,  empresaId,  data);

                var response = agenda.Select(a => new AgendamentoAgendaDiaDto
                {
                    NomeCliente = a.Cliente.Nome,
                    NomeServico = a.Servico.Nome,
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
    }
}
