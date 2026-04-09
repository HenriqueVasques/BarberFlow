using BarberFlow.API.DTOs.Empresa;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly EmpresaService _empresaService;

        public EmpresaController(EmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        #region Operações de Escrita (Ações de Comando)

        // Cria uma nova empresa/barbearia no sistema
        [HttpPost]
        public async Task<IActionResult> CriarEmpresa([FromBody] EmpresaCreateDto dto)
        {
            try
            {
                var empresa = await _empresaService.CriarEmpresa(dto);
                return StatusCode(201, new { message = "Empresa criada com sucesso!", dados = empresa });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Atualiza os dados principais da empresa
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEmpresa(long id, [FromBody] EmpresaUpdateDto dto)
        {
            try
            {
                await _empresaService.AtualizarEmpresa(id, dto);
                return Ok(new { message = "Empresa atualizada com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Realiza a exclusão lógica da empresa
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarEmpresa(long id)
        {
            try
            {
                await _empresaService.Deletar(id);
                return Ok(new { message = "Empresa removida com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Operações de Leitura (Consultas)

        // Busca uma empresa pelo Slug (usado no link público de agendamento)
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> ObterEmpresaPorSlug(string slug)
        {
            try
            {
                var empresa = await _empresaService.ObterEmpresaPorSlug(slug);
                return Ok(new { message = "Empresa encontrada com sucesso!", dados = empresa });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Busca uma empresa pelo ID (usado na administração)
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            try
            {
                var empresa = await _empresaService.ObterPorId(id);
                return Ok(new { message = "Empresa encontrada com sucesso!", dados = empresa });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion
    }
}