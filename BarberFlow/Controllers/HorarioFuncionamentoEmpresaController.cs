using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BarberFlow.API.DTOs;
using BarberFlow.API.Services;
using BarberFlow.API.Models;

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

        #region Rotas: Admin (Painel de Gestão)

        // Aplica a trava de Admin para todo esse bloco
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/criar/{empresaId}")]
        public async Task<IActionResult> CriarHorarioFuncionamentoEmpresa(long empresaId, [FromBody] HorarioFuncionamentoEmpresaCreateDto dto)
        {
            try
            {
                var horarioFuncionamentoEmpresa = await _horarioFuncionamentoEmpresaService.CriarHorarioFuncionamentoEmpresa(dto, empresaId);


                var response = MapearParaResponseDto(horarioFuncionamentoEmpresa);
                return StatusCode(201, new
                {
                    message = "Horario Funcionamento da Empresa criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }         
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/atualizar/{id}")]
        public async Task<IActionResult> AtualizarHorarioFuncionamentoEmpresa(long id, [FromBody] HorarioFuncionamentoEmpresaUpadteDto dto)
        {
            try
            {
                var horarioFuncionamentoEmpresa = await _horarioFuncionamentoEmpresaService.AtualizarHorarioFuncionamentoEmpresa(dto, id);

                var response = MapearParaResponseDto(horarioFuncionamentoEmpresa);
                return StatusCode(200, new
                {
                    message = "Horario Funcionamento da Empresa atualizado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });

            }         
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/remover/{id}")]
        public async Task<IActionResult> DeletarHorarioFuncionamentoEmpresa(long id)
        {
            try
            {
                var horarioFuncionamentoEmpresa = await _horarioFuncionamentoEmpresaService.DeletarHorarioFuncionamentoEmpresa(id);

                var response = MapearParaResponseDto(horarioFuncionamentoEmpresa);
                return StatusCode(200, new
                {
                    message = "Horario Funcionamento da Empresa deletado com sucesso!",
                    dados = response
                });
            }   
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message});
            }         
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/lista-completa/{empresaId}")]
        public async Task<IActionResult> ObterTodosPorEmpresaParaAdmin(long empresaId)
        {
            try
            {
                // Aqui o service chama o repo com apenasAtivos: false
                var horarioFuncionamentoEmpresa = await _horarioFuncionamentoEmpresaService.ObterTodosPorEmpresaParaAdmin(empresaId);

                var response = horarioFuncionamentoEmpresa.Select(horarioFuncionamentoEmpresa => new HorarioFuncionamentoEmpresaResponseDto
                {
                    Id = horarioFuncionamentoEmpresa.Id,
                    NomeEmpresa = horarioFuncionamentoEmpresa.Empresa?.Nome ?? "N/A",
                    DiaSemana = horarioFuncionamentoEmpresa.DiaSemana,
                    EstaFechado = horarioFuncionamentoEmpresa.EstaFechado,
                    HoraAbertura = horarioFuncionamentoEmpresa.HoraAbertura,
                    HoraFechamento = horarioFuncionamentoEmpresa.HoraFechamento,
                });
                return StatusCode(200, new
                {
                    message = "Horario Funcionamento da Empresa Recuperado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }   

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/obter-pelo-id/{id}")]
        public async Task<IActionResult> ObterPorIdAdmin(long id)
        {
            try
            {
                // Aqui o service chama o repo com apenasAtivos: false
                var horarioFuncionamentoEmpresa = await _horarioFuncionamentoEmpresaService.ObterPorIdAdmin(id);

                var response = MapearParaResponseDto(horarioFuncionamentoEmpresa);
                return StatusCode(200, new
                {
                    message = "Horario Funcionamento da Empresa Recuperado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion

        #region Rotas: Cliente (App)

        [HttpGet("empresa/{empresaId}")]
        // Qualquer usuário logado pode ver os horários disponíveis para agendar
        public async Task<IActionResult> ObterTodosPorEmpresaCliente(long empresaId)
        {
            try
            {
                var horarioFuncionamentoEmpresa = await _horarioFuncionamentoEmpresaService.ObterTodosPorEmpresaCliente(empresaId);

                var response = horarioFuncionamentoEmpresa.Select(horarioFuncionamentoEmpresa => new HorarioFuncionamentoEmpresaResponseDto
                {
                    Id = horarioFuncionamentoEmpresa.Id,
                    NomeEmpresa = horarioFuncionamentoEmpresa.Empresa?.Nome ?? "N/A",
                    DiaSemana = horarioFuncionamentoEmpresa.DiaSemana,
                    EstaFechado = horarioFuncionamentoEmpresa.EstaFechado,
                    HoraAbertura = horarioFuncionamentoEmpresa.HoraAbertura,
                    HoraFechamento = horarioFuncionamentoEmpresa.HoraFechamento,
                });
                return StatusCode(200, new
                {
                    message = "Horario Funcionamento da Empresa criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Private Methods
        private static HorarioFuncionamentoEmpresaResponseDto MapearParaResponseDto(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa)
        {
            return new HorarioFuncionamentoEmpresaResponseDto
            {
                Id = horarioFuncionamentoEmpresa.Id,
                NomeEmpresa = horarioFuncionamentoEmpresa.Empresa?.Nome ?? "N/A",
                DiaSemana = horarioFuncionamentoEmpresa.DiaSemana,
                EstaFechado = horarioFuncionamentoEmpresa.EstaFechado,
                HoraAbertura = horarioFuncionamentoEmpresa.HoraAbertura,
                HoraFechamento = horarioFuncionamentoEmpresa.HoraFechamento,
            };
        }
        #endregion
    }
}