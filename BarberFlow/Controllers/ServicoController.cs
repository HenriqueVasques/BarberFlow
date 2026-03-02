using BarberFlow.API.DTOs.Servico;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

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
                if(dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }
                var novoServico = await _servicoService.CriarServico(dto);
                
                var response = new ServicoResponseDto 
                {
                    Nome = novoServico.Nome,
                    DuracaoMinutos = novoServico.DuracaoMinutos,
                    PrecoBase = novoServico.PrecoBase,
                    DataCriacao = novoServico.DataCriacao
                };
                return StatusCode(201, new 
                {
                    message = "Serviço criado com sucesso!",
                    dados = response
                });
            }
            catch(Exception ex)
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
                    Nome = s.Nome,
                    DuracaoMinutos = s.DuracaoMinutos,
                    PrecoBase = s.PrecoBase,
                    DataCriacao = s.DataCriacao
                });
                return Ok(new 
                {
                    message = "Serviços obtidos com sucesso!",
                    dados = response
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            };
        }
    }
}
