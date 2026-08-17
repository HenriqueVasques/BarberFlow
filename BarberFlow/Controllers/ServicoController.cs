using BarberFlow.API.DTOs.Servico;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly ServicoService _servicoService;

        public ServicoController(ServicoService servicoService)
        {
            _servicoService = servicoService;
        }

        #region Endpoints Administrativos (Admin)

        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CriarServico([FromBody] ServicoCreateDto dto)
        {
            try
            {
                var servico = await _servicoService.CriarServico(dto);
                return StatusCode(201, new { message = "Serviço criado com sucesso!", dados = servico });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarServico(long id, [FromBody] ServicoUpdateDto dto)
        {
            try
            {
                await _servicoService.AtualizarServico(id, dto);
                return Ok(new { message = "Serviço atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarServico(long id)
        {
            try
            {
                await _servicoService.DeletarServico(id);
                return Ok(new { message = "Serviço deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("admin/obter-por-empresa/{empresaId}")]
        public async Task<IActionResult> ObterServicosPorEmpresaAdmin(long empresaId)
        {
            try
            {
                var servicos = await _servicoService.ObterServicosPorEmpresaAdmin(empresaId);
                return Ok(new { message = "Serviços recuperados com sucesso!", dados = servicos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Endpoints Públicos / Cliente

        [Authorize(Roles = "Cliente, Administrador")]
        [HttpGet("cliente/obter-por-empresa/{empresaId}")]
        public async Task<IActionResult> ObterServicosPorEmpresaCliente(long empresaId)
        {
            try
            {
                var servicos = await _servicoService.ObterServicosPorEmpresaCliente(empresaId);
                return Ok(new { message = "Serviços recuperados com sucesso!", dados = servicos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion
    }
}