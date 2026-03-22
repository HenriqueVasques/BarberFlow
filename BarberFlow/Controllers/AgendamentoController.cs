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

        #region Operações de Escrita (Ações de Comando)

        // Solicita a criação de um novo agendamento após validações de negócio
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

        // Altera o status de um agendamento para 'Cancelado'
        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> CancelarAgendamento(long id)
        {
            try
            {
                var agendamento = await _agendamentoService.Cancelar(id);
                var response = MapearParaResponseDto(agendamento);

                return Ok(new { message = "Agendamento cancelado com sucesso!", dados = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Altera o status para 'Finalizado', concluindo o ciclo de atendimento
        [HttpPatch("{id}/finalizar")]
        public async Task<IActionResult> Finalizar(long id)
        {
            try
            {
                var agendamento = await _agendamentoService.Finalizar(id);
                var response = MapearParaResponseDto(agendamento);

                return Ok(new { message = "Agendamento finalizado com sucesso!", dados = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Visão: Geral (Consulta Básica)

        // Recupera os detalhes completos de um agendamento específico
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

        #endregion

        #region Visão: Cliente

        // Busca o próximo compromisso ativo para exibição no dashboard do cliente
        [HttpGet("cliente/{clienteId}/proximo-agendamento")]
        public async Task<IActionResult> ObterProximoAgendamentoCliente(long clienteId)
        {
            try
            {
                var proximo = await _agendamentoService.ObterProximoAgendamentoCliente(clienteId);

                if (proximo == null)
                    return Ok(new
                    {
                        message = "Nenhum agendamento futuro encontrado.",
                        dados = ""
                    });

                var response = MapearParaResponseDto(proximo);
                return Ok(new { message = "Próximo agendamento recuperado!", dados = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Retorna o histórico de atendimentos realizados pelo cliente
        [HttpGet("cliente/{clienteId}/historico")]
        public async Task<IActionResult> ObterHistoricoCliente(long clienteId)
        {
            try
            {
                var agendamentos = await _agendamentoService.ObterHistoricoCliente(clienteId);
                var response = agendamentos.Select(MapearParaResponseDto).ToList();

                return Ok(new
                {
                    message = "Histórico do cliente recuperado!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Visão: Profissional (Agenda de Trabalho)

        // Lista a agenda de um profissional específico para um período determinado
        [HttpGet("professional/agenda")]
        public async Task<IActionResult> ObterAgendaProfissional([FromQuery] long profissionalId, [FromQuery] long empresaId, [FromQuery] DateTime inicio, [FromQuery] DateTime fim, [FromQuery] List<StatusAgendamento> status)
        {
            try
            {
                if (status == null || !status.Any())
                    status = new List<StatusAgendamento> { StatusAgendamento.Pendente, StatusAgendamento.Confirmado };

                var agenda = await _agendamentoService.ObterAgendaPorPeriodo(profissionalId, empresaId, inicio, fim, status);
                var response = agenda.Select(MapearParaResponseDto).ToList();

                return Ok(new
                {
                    message = "Agenda do profissional obtida!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Visão: Admin (Dashboard e Relatórios Gerais)

        // Consolida faturamento e volume de atendimentos de um dia específico
        [HttpGet("admin/{empresaId}/faturamento-diario")]
        public async Task<IActionResult> ObterResumoPorDia(long empresaId, DateTime data)
        {
            try
            {
                var response = await _agendamentoService.ObterResumoPorDia(empresaId, data);
                return Ok(new
                {
                    message = "Resumo financeiro obtido!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Gera relatório geral de agendamentos de toda a empresa (visão gerencial)
        [HttpGet("admin/{empresaId}/historico-geral")]
        public async Task<IActionResult> HistoricoGeralAdmin(long empresaId, [FromQuery] DateTime inicio, [FromQuery] DateTime fim, [FromQuery] List<StatusAgendamento> status)
        {
            try
            {
                if (status == null || !status.Any())
                    status = new List<StatusAgendamento> { StatusAgendamento.Confirmado, StatusAgendamento.Finalizado };

                var agenda = await _agendamentoService.ObterAgendaPorPeriodo(null, empresaId, inicio, fim, status);
                var response = agenda.Select(MapearParaResponseDto).ToList();

                return Ok(new
                {
                    message = "Relatório geral da empresa obtido!",
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

        // Converte a entidade de banco para o DTO de resposta, protegendo dados sensíveis
        private static AgendamentoResponseDto MapearParaResponseDto(Agendamento agendamento)
        {
            if (agendamento == null) return null;

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