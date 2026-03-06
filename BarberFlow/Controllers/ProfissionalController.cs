using BarberFlow.API.DTOs.Profissional;
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
                if(dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }

                var profissional = await _profissionalService.CriarProfissional(dto);

                if (profissional == null)
                {
                    return BadRequest("Não foi possível criar o profissional.");
                }

                var response = new ProfissionalResponseDto
                {
                    EmpresaId = profissional.EmpresaId,
                    UsuarioId = profissional.UsuarioId,
                    PercentualComissao = profissional.PercentualComissao,
                    DataCriacao = profissional.DataCriacao,
                    DataAtualizacao = profissional.DataAtualizacao,
                    IsDeleted = profissional.IsDeleted,
                    Ativo = profissional.Ativo
                };

                return StatusCode(201, new 
                {
                    message = "Profissional criado com sucesso!",
                    dados = response
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarProfissional(long id, [FromBody] ProfissionalUpdateDto dto)
        {
            try
            {
                if(dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }
                var profissional = await _profissionalService.AtualizarProfissional(id, dto);
                if (profissional == null)
                {
                    return NotFound("Profissional não encontrado.");
                }
                var response = new ProfissionalResponseDto
                {
                    Nome = profissional.Usuario.Nome,
                    Email = profissional.Usuario.Email,
                    EmpresaId = profissional.EmpresaId,
                    UsuarioId = profissional.UsuarioId,
                    PercentualComissao = profissional.PercentualComissao,
                    DataCriacao = profissional.DataCriacao,
                    DataAtualizacao = profissional.DataAtualizacao,
                    IsDeleted = profissional.IsDeleted,
                    Ativo = profissional.Ativo
                };
                return Ok(new 
                {
                    message = "Profissional atualizado com sucesso!",
                    dados = response
                });
            }
            catch(Exception ex)
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
                {
                    return NotFound("Profissional não encontrado.");
                }

                var response = new ProfissionalResponseDto
                {
                    Nome = profissional.Usuario.Nome,
                    Email = profissional.Usuario.Email,
                    EmpresaId = profissional.EmpresaId,
                    UsuarioId = profissional.UsuarioId,
                    PercentualComissao = profissional.PercentualComissao,
                    DataCriacao = profissional.DataCriacao,
                    DataAtualizacao = profissional.DataAtualizacao,
                    IsDeleted = profissional.IsDeleted,
                    Ativo = profissional.Ativo
                };

                return Ok(new
                {
                    message = "Profissional deletado com sucesso!",
                    dados = response
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
    }
}
