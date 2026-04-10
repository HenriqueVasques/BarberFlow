using Microsoft.AspNetCore.Mvc;
using BarberFlow.API.DTOs;
using BarberFlow.API.Services;

namespace BarberFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioFuncionamentoController : ControllerBase
    {
        private readonly HorarioFuncionamentoEmpresaService _horarioFuncionamentoEmpresaService;

        public HorarioFuncionamentoController(HorarioFuncionamentoEmpresaService horarioFuncionamentoEmpresaService)
        {
            _horarioFuncionamentoEmpresaService = horarioFuncionamentoEmpresaService;
        }

        #region Comandos: Admin (Escrita)

        // Cria uma nova configuração de horário para uma empresa específica
        [HttpPost("admin/criar/{empresaId}")]
        public async Task<IActionResult> CriarHorarioFuncionamentoEmpresa(long empresaId, [FromBody] HorarioFuncionamentoEmpresaCreateDto dto)
        {
            try
            {
                var horarioFuncionamentoEmpresa = await _horarioFuncionamentoEmpresaService.CriarHorarioFuncionamentoEmpresa(dto, empresaId);
                return StatusCode(201, new { message = "Horário de funcionamento criado com sucesso!", dados = horarioFuncionamentoEmpresa });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Atualiza os dados de um horário de funcionamento existente
        [HttpPut("admin/atualizar/{id}")]
        public async Task<IActionResult> AtualizarHorarioFuncionamentoEmpresa(long id, [FromBody] HorarioFuncionamentoEmpresaUpadteDto dto)
        {
            try
            {
                await _horarioFuncionamentoEmpresaService.AtualizarHorarioFuncionamentoEmpresa(dto, id);
                return Ok(new { message = "Horário de funcionamento atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Remove (soft delete) uma configuração de horário do sistema
        [HttpDelete("admin/remover/{id}")]
        public async Task<IActionResult> DeletarHorarioFuncionamentoEmpresa(long id)
        {
            try
            {
                await _horarioFuncionamentoEmpresaService.DeletarHorarioFuncionamentoEmpresa(id);
                return Ok(new { message = "Horário de funcionamento removido com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Consultas: Painel Administrativo (Leitura)

        // Lista todos os horários ativos de uma empresa específica
        [HttpGet("admin/por-empresa/{empresaId}")]
        public async Task<IActionResult> ObterPorEmpresa(long empresaId)
        {
            try
            {
                var dados = await _horarioFuncionamentoEmpresaService.ObterPorEmpresa(empresaId, apenasAtivos: true, incluirDeletados: false);
                return Ok(new { message = "Dados recuperados com sucesso!", dados });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Retorna o histórico completo (incluindo inativos e deletados) de uma empresa
        [HttpGet("admin/historico/{empresaId}")]
        public async Task<IActionResult> ObterHistoricoPorEmpresa(long empresaId)
        {
            try
            {
                var dados = await _horarioFuncionamentoEmpresaService.ObterPorEmpresa(empresaId, apenasAtivos: false, incluirDeletados: true);
                return Ok(new { message = "Histórico recuperado com sucesso!", dados });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Obtém detalhes de um horário específico pelo ID
        [HttpGet("admin/obter-pelo-id/{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            try
            {
                var dados = await _horarioFuncionamentoEmpresaService.ObterPorId(id, apenasAtivos: true, incluirDeletados: false);
                return Ok(new { message = "Horário recuperado com sucesso!", dados });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Obtém o histórico de um horário específico (mesmo se deletado)
        [HttpGet("admin/obter-pelo-id-historico/{id}")]
        public async Task<IActionResult> ObterPorIdHistorico(long id)
        {
            try
            {
                var dados = await _horarioFuncionamentoEmpresaService.ObterPorId(id, apenasAtivos: false, incluirDeletados: true);
                return Ok(new { message = "Dados históricos recuperados com sucesso!", dados });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Busca o horário ativo de um dia da semana para uma empresa
        [HttpGet("admin/obter-por-dia/{empresaId}")]
        public async Task<IActionResult> ObterPorDia(long empresaId, [FromQuery] DayOfWeek diaDaSemana)
        {
            try
            {
                var dados = await _horarioFuncionamentoEmpresaService.ObterPorDia(empresaId, diaDaSemana, apenasAtivos: true, incluirDeletados: false);
                return Ok(new { message = "Horário do dia recuperado com sucesso!", dados });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion
    }
}