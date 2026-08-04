using BarberFlow.API.DTOs.ProfissionalServico;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfissionalServicoController : ControllerBase
    {
        private readonly ProfissionalServicoService _profissionalServicoService;

        public ProfissionalServicoController(ProfissionalServicoService profissionalServicoService)
        {
            _profissionalServicoService = profissionalServicoService;
        }

        #region Endpoints Administrativos (Admin)

        //[Authorize(Roles = "Admin")]
        [HttpPost("admin/criar")]
        public async Task<IActionResult> CriarProfissionalServico([FromBody] ProfissionalServicoCreateDto dto)
        {
            try
            {
                var profissionalServico = await _profissionalServicoService.CriarProfissionalServico(dto);
                return StatusCode(201, new { message = "Serviço do Profissional criado com sucesso!", dados = profissionalServico });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("admin/atualizar/{id}")]
        public async Task<IActionResult> AtualizarProfissionalServico(long id, [FromBody] ProfissionalServicoUpdateDto dto)
        {
            try
            {
                await _profissionalServicoService.AtualizarProfissionalServico(id, dto);
                return Ok(new { message = "Serviço do Profissional atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("admin/remover/{id}")]
        public async Task<IActionResult> DeletarProfissionalServico(long id)
        {
            try
            {
                await _profissionalServicoService.DeletarProfissionalServico(id);
                return Ok(new { message = "Serviço do Profissional deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("admin/obter-pelo-id/{id}")]
        public async Task<IActionResult> ObterPorIdAdmin(long id)
        {
            try
            {
                var profissionalServico = await _profissionalServicoService.ObterPorIdAdmin(id);
                return Ok(new { message = "Serviço do Profissional recuperado com sucesso!", dados = profissionalServico });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("admin/obter-pelo-profissional-id/{profissionalId}")]
        public async Task<IActionResult> ObterPorProfissionalIdAdmin(long profissionalId)
        {
            try
            {
                var serviços = await _profissionalServicoService.ObterPorProfissionalIdAdmin(profissionalId);
                return Ok(new { message = "Serviços do Profissional recuperados com sucesso!", dados = serviços });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Endpoints Públicos / Cliente

        //[Authorize(Roles = "Cliente")]
        [HttpGet("cliente/obter-pelo-id/{id}")]
        public async Task<IActionResult> ObterPorIdCliente(long id)
        {
            try
            {
                var profissionalServico = await _profissionalServicoService.ObterPorIdCliente(id);
                return Ok(new { message = "Serviço do Profissional recuperado com sucesso!", dados = profissionalServico });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Cliente")]
        [HttpGet("cliente/obter-pelo-profissional-id/{profissionalId}")]
        public async Task<IActionResult> ObterPorProfissionalIdCliente(long profissionalId)
        {
            try
            {
                var serviços = await _profissionalServicoService.ObterPorProfissionalIdCliente(profissionalId);
                return Ok(new { message = "Serviços do Profissional recuperados com sucesso!", dados = serviços });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion
    }
}