using BarberFlow.API.DTOs.Servico;
using BarberFlow.API.Models;
using BarberFlow.API.Services;
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

        [HttpPost]
        public async Task<IActionResult> CriarServico([FromBody] ServicoCreateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }
                var servico = await _servicoService.CriarServico(dto);

                var response = MapearParaResponseDto(servico);
                return StatusCode(201, new
                {
                    message = "Serviço criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarServico(long id, [FromBody] ServicoUpdateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }

                var servico = await _servicoService.AtualizarServico(id, dto);

                if (servico == null)
                {
                    return NotFound(new { message = $"Serviço com id '{id}' não encontrado." });
                }

                var response = MapearParaResponseDto(servico);
                return Ok(new
                {
                    message = "Serviço atualizado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarServico(long id)
        {
            try
            {
                var servico = await _servicoService.DeletarServico(id);

                if (servico == null)
                {
                    return NotFound(new { message = $"Serviço com id '{id}' não encontrado." });
                }

                var response = MapearParaResponseDto(servico);
                return Ok(new
                {
                    message = "Serviço deletado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterServicosPorEmpresa([FromQuery] long empresaId)
        {
            try
            {
                var servicos = await _servicoService.ObterServicosPorEmpresa(empresaId);
                var response = servicos.Select(s => new ServicoResponseDto
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    NomeEmpresa = s.Empresa.Nome,
                    DuracaoMinutos = s.DuracaoMinutos,
                    PrecoBase = s.PrecoBase,
                    DataCriacao = s.DataCriacao,
                    IsDeleted = s.IsDeleted,
                    Ativo = s.Ativo
                });
                return Ok(new
                {
                    message = "Serviços obtidos com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }

        private static ServicoResponseDto MapearParaResponseDto(Servico servico)
        {
            return new ServicoResponseDto
            {
                Id = servico.Id,
                Nome = servico.Nome,
                NomeEmpresa = servico.Empresa?.Nome ?? "N/A",
                DuracaoMinutos = servico.DuracaoMinutos,
                PrecoBase = servico.PrecoBase,
                DataCriacao = servico.DataCriacao,
                DataAtualizacao = servico.DataAtualizacao,
                IsDeleted = servico.IsDeleted,
                Ativo = servico.Ativo
            };
        }
    } 
}
