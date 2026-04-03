using BarberFlow.API.DTOs.BloqueioHorario;
using BarberFlow.API.Services;
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

        #region Operações de Escrita (Ações de Comando)

        // Solicita a criação de um novo bloqueio de horário (ex: folgas, almoço, manutenção)
        [HttpPost]
        public async Task<IActionResult> CriarBloqueioHorario([FromBody] BloqueioHorarioCreateDto dto)
        {
            try
            {
                var bloqueioHorario = await _bloqueioHorarioService.CriarBloqueioHorario(dto);

                return CreatedAtAction(nameof(ObterPorEmpresaId), new { empresaId = dto.EmpresaId }, new
                {
                    message = "Bloqueio de horário criado com sucesso!",
                    dados = bloqueioHorario
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Altera informações de um bloqueio existente, validando novos conflitos de agenda
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarBloqueioHorario(long id, [FromBody] BloqueioHorarioUpdateDto dto)
        {
            try
            {
                await _bloqueioHorarioService.AtualizarBloqueioHorario(id, dto);
                return Ok(new { message = "Bloqueio de horário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Realiza a exclusão lógica do bloqueio, liberando o horário na agenda do profissional
        [HttpDelete("{id}")]
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

        #endregion

        #region Visão: Admin (Gestão da Empresa)

        // Recupera os bloqueios ativos de todos os profissionais de uma empresa
        [HttpGet("{empresaId}/por-empresa")]
        public async Task<IActionResult> ObterPorEmpresaId(long empresaId, [FromQuery] DateOnly inicio, [FromQuery] DateOnly fim, [FromQuery] int pagina = 1)
        {
            try
            {
                var bloqueioHorario = await _bloqueioHorarioService.ObterPorEmpresaId(empresaId, inicio, fim, pagina, incluirDeletados: false);

                return Ok(new
                {
                    message = "Bloqueios ativos da empresa obtidos com sucesso!",
                    dados = bloqueioHorario
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Recupera o histórico completo (incluindo removidos) para auditoria e relatórios
        [HttpGet("{empresaId}/historico-por-empresa")]
        public async Task<IActionResult> ObterHistoricoPorEmpresaId(long empresaId, [FromQuery] DateOnly inicio, [FromQuery] DateOnly fim, [FromQuery] int pagina = 1)
        {
            try
            {
                var bloqueioHorario = await _bloqueioHorarioService.ObterPorEmpresaId(empresaId, inicio, fim, pagina, incluirDeletados: true);

                return Ok(new
                {
                    message = "Histórico de bloqueios da empresa obtido com sucesso!",
                    dados = bloqueioHorario
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Visão: Profissional (Agenda Pessoal)

        // Lista os bloqueios ativos na agenda de um profissional específico
        [HttpGet("{profissionalId}/por-profissional")]
        public async Task<IActionResult> ObterPorProfissionalId(long profissionalId, [FromQuery] DateOnly inicio, [FromQuery] DateOnly fim, [FromQuery] int pagina = 1)
        {
            try
            {
                var bloqueioHorario = await _bloqueioHorarioService.ObterPorProfissionalId(profissionalId, inicio, fim, pagina, incluirDeletados: false);

                return Ok(new
                {
                    message = "Bloqueios ativos do profissional obtidos com sucesso!",
                    dados = bloqueioHorario
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Lista o histórico de bloqueios (ativos e deletados) de um profissional específico
        [HttpGet("{profissionalId}/historico-por-profissional")]
        public async Task<IActionResult> ObterHistoricoPorProfissionalId(long profissionalId, [FromQuery] DateOnly inicio, [FromQuery] DateOnly fim, [FromQuery] int pagina = 1)
        {
            try
            {
                var bloqueioHorario = await _bloqueioHorarioService.ObterPorProfissionalId(profissionalId, inicio, fim, pagina, incluirDeletados: true);

                return Ok(new
                {
                    message = "Histórico de bloqueios do profissional obtido com sucesso!",
                    dados = bloqueioHorario
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