using BarberFlow.API.DTOs.Profissional;
using BarberFlow.API.Models;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfissionalController : ControllerBase
    {
        private readonly ProfissionalService _profissionalService;

        public ProfissionalController(ProfissionalService profissionalService)
        {
            _profissionalService = profissionalService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarProfissional([FromBody] ProfissionalCreateDto dto)
        {
            try
            {
                var profissional = await _profissionalService.CriarProfissional(dto);

                if (profissional == null)
                {
                    return BadRequest("Não foi possível criar o profissional.");
                }

                var response = MapearParaResponseDto(profissional);
                return StatusCode(201, new
                {
                    message = "Profissional criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarProfissional(long id, [FromBody] ProfissionalUpdateDto dto)
        {
            try
            {
                var profissional = await _profissionalService.AtualizarProfissional(id, dto);
                if (profissional == null)
                {
                    return NotFound("Profissional não encontrado.");
                }
                var response = MapearParaResponseDto(profissional);
                return Ok(new
                {
                    message = "Profissional atualizado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarProfissional(long id)
        {
            try
            {
                var profissional = await _profissionalService.DeletarProfissional(id);
                if (profissional == null)
                    return NotFound("Profissional não encontrado.");

                var response = MapearParaResponseDto(profissional);
                return Ok(new
                {
                    message = "Profissional deletado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("Profissionais-Por-Empresa")]
        public async Task<IActionResult> ObterProfissionaisPorEmpresa(long empresaId)
        {
            try
            {
                var profissionais = await _profissionalService.ObterProfissionaisPorEmpresa(empresaId);

                var response = profissionais.Select(profissional => new ProfissionalResponseDto
                {
                    Id = profissional.Id,
                    Nome = profissional.Usuario.Nome,
                    NomeEmpresa = profissional.Empresa.Nome,
                    Email = profissional.Usuario.Email,
                    EmpresaId = profissional.EmpresaId,
                    UsuarioId = profissional.UsuarioId,
                    PercentualComissao = profissional.PercentualComissao,
                    DataCriacao = profissional.DataCriacao,
                    Ativo = profissional.Ativo
                }).ToList();
                return Ok(new
                {
                    message = "Profissionais obtidos com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("Profissional-Por-Id")]
        public async Task<IActionResult> ObterProfissionalPorId(long id)
        {
            try
            {
                var profissional = await _profissionalService.ObterPorId(id);
                if (profissional == null)
                {
                    return NotFound("Profissional não encontrado.");
                }
                var response = MapearParaResponseDto(profissional);
                return Ok(new
                {
                    message = "Profissional obtido com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private static ProfissionalResponseDto MapearParaResponseDto(Profissional profissional)
        {
            return new ProfissionalResponseDto
            {
                Id = profissional.Id,
                Nome = profissional.Usuario?.Nome ?? "N/A",
                NomeEmpresa = profissional.Empresa?.Nome ?? "N/A",
                Email = profissional.Usuario?.Email ?? "N/A",
                EmpresaId = profissional.EmpresaId,
                UsuarioId = profissional.UsuarioId,
                PercentualComissao = profissional.PercentualComissao,
                DataCriacao = profissional.DataCriacao,
                Ativo = profissional.Ativo
            };
        }
    }
}