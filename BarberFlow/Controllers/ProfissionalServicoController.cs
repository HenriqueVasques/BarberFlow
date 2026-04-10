using BarberFlow.API.DTOs;
using BarberFlow.API.Models;
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

        //[Authorize(Roles = "Admin")]
        [HttpPost("admin/criar/")]
        public async Task<IActionResult> CriarProfissionalServico(ProfissionalServicoCreateDto dto)
        {
            try
            {
                var profissionalServico = await _profissionalServicoService.CriarProfissionalServico(dto);
                var response = MapearParaResponseDto(profissionalServico);
                return StatusCode(201, new
                {
                    message = "Serviço do Profissional criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("admin/atualizar/{id}")]
        public async Task<IActionResult> AtualizarProfissionalServico(long id, ProfissionalServicoUpdateDto dto)
        {
            try
            {
                var profissionalServico = await _profissionalServicoService.AtualizarProfissionalServico(id, dto);
                var response = MapearParaResponseDto(profissionalServico);
                return StatusCode(201, new
                {
                    message = "Serviço do Profissional atualizado com sucesso!",
                    dados = response
                });
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
                var profissionalServico = await _profissionalServicoService.DeletarProfissionalServico(id);
                var response = MapearParaResponseDto(profissionalServico);
                return StatusCode(201, new
                {
                    message = "Serviço do Profissional deletado com sucesso!",
                    dados = response
                });
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
                var response = MapearParaResponseDto(profissionalServico);
                return StatusCode(201, new
                {
                    message = "Serviço do Profissional recuperado com sucesso!",
                    dados = response
                });
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
                var profissionalServico = await _profissionalServicoService.ObterPorProfissionalIdAdmin(profissionalId);
                var response = profissionalServico.Select(ps => new ProfissionalServicoResponseDto
                {
                    Id = ps.Id,
                    ProfissionalId = ps.ProfissionalId,
                    ServicoId = ps.ServicoId,
                    NomeProfisional = ps.Profissional.Usuario.Nome,
                    NomeServico = ps.Servico.Nome,
                    PrecoPersonalizado = ps.PrecoPersonalizado,
                    DuracaoPersonalizadaMinutos = ps.DuracaoPersonalizadaMinutos,
                    Ativo = ps.Ativo,
                });
                return StatusCode(201, new
                {
                    message = "Serviços do Profissional recuperado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //[Authorize(Roles = "Cliente")]
        [HttpGet("cliente/obter-pelo-id/{id}")]
        public async Task<IActionResult> ObterPorIdCliente(long id)
        {
            try
            {
                var profissionalServico = await _profissionalServicoService.ObterPorIdCliente(id);
                var response = MapearParaResponseDto(profissionalServico);
                return StatusCode(201, new
                {
                    message = "Serviço do Profissional recuperado com sucesso!",
                    dados = response
                });
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
                var profissionalServico = await _profissionalServicoService.ObterPorProfissionalIdCliente(profissionalId);
                var response = profissionalServico.Select(ps => new ProfissionalServicoResponseDto
                {
                    Id = ps.Id,
                    ProfissionalId = ps.ProfissionalId,
                    ServicoId = ps.ServicoId,
                    NomeProfisional = ps.Profissional.Usuario.Nome,
                    NomeServico = ps.Servico.Nome,
                    PrecoPersonalizado = ps.PrecoPersonalizado,
                    DuracaoPersonalizadaMinutos = ps.DuracaoPersonalizadaMinutos,
                    Ativo = ps.Ativo,
                });
                return StatusCode(201, new
                {
                    message = "Serviços do Profissional recuperado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        private static ProfissionalServicoResponseDto MapearParaResponseDto(ProfissionalServico profissionalServico)
        {
            return new ProfissionalServicoResponseDto
            {
                Id = profissionalServico.Id,
                ProfissionalId = profissionalServico.ProfissionalId,
                ServicoId = profissionalServico.ServicoId,
                NomeProfisional = profissionalServico.Profissional?.Usuario?.Nome ?? "N/A",
                NomeServico = profissionalServico.Servico?.Nome ?? "N/A",
                PrecoPersonalizado = profissionalServico.PrecoPersonalizado,
                DuracaoPersonalizadaMinutos = profissionalServico.DuracaoPersonalizadaMinutos,
                Ativo = profissionalServico.Ativo,
            };
        }
    }
}
