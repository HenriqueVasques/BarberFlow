using BarberFlow.API.DTOs.Empresa;
using BarberFlow.API.Models;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        #region Readonly Fields
        private readonly EmpresaService _empresaService;
        #endregion

        #region Constructor
        public EmpresaController(EmpresaService empresaService)
        {
            _empresaService = empresaService;
        }
        #endregion

        #region Endpoints
        [HttpPost]
        public async Task<IActionResult> CriarEmpresa([FromBody] EmpresaCreateDto dto)
        {
            try
            {
                if(dto == null)
                {
                    return BadRequest("Dados inválidos.");
                }
                var novaEmpresa = await _empresaService.CriarEmpresa(dto);

                var response = new EmpresaResponseDto
                {
                    Id = novaEmpresa.Id,
                    Nome = novaEmpresa.Nome,
                    Slug = novaEmpresa.Slug,
                    CNPJ = novaEmpresa.CNPJ,
                    DataCriacao = novaEmpresa.DataCriacao
                };
                return StatusCode(201, new
                {
                    message = "Empresa criada com sucesso!",
                    dados = response
                });
            }
            catch(Exception ex) {

                return BadRequest(new { error = ex.Message });
            }     
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> ObterEmpresaPorSlug([FromRoute] string slug)
        {
            
            try
            {
                var empresa = await _empresaService.ObterEmpresaPorSlug(slug);

                if (empresa == null)
                {
                    return NotFound(new { message = $"Empresa com slug '{slug}' não encontrada." });
                }

                var response = new EmpresaResponseDto
                {
                    Id = empresa.Id,
                    Nome = empresa.Nome,
                    Slug = empresa.Slug,
                    CNPJ = empresa.CNPJ,
                    DataCriacao = empresa.DataCriacao
                };

                return Ok(new
                {
                    message = "Empresa encontrada com sucesso!",
                    dados = response
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }    
        }
        #endregion
    }
}
