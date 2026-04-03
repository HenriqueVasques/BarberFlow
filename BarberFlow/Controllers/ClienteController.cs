using BarberFlow.API.DTOs.Cliente;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        #region Operações de Escrita (Ações de Comando)

        // Solicita a criação de um novo cliente no sistema
        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromBody] ClienteCreateDto dto)
        {
            try
            {
                var cliente = await _clienteService.CriarCliente(dto);
                return StatusCode(201, new { message = "Cliente criado com sucesso!", dados = cliente });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Altera os dados cadastrais de um cliente existente
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCliente(long id, [FromBody] ClienteUpdateDto dto)
        {
            try
            {
                await _clienteService.AtualizarCliente(id, dto);
                return Ok(new { message = "Cliente atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Realiza a exclusão lógica do cliente do sistema
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCliente(long id)
        {
            try
            {
                await _clienteService.DeletarCliente(id);
                return Ok(new { message = "Cliente deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Operações de Leitura (Consultas)

        // Recupera a lista de clientes ativos de uma empresa específica com paginação
        [HttpGet("empresa/{empresaId}/clientes")]
        public async Task<IActionResult> ObterClientesPorEmpresa(long empresaId, [FromQuery] int pagina = 1)
        {
            try
            {
                var clientes = await _clienteService.ObterClientesPorEmpresa(empresaId, incluirDeletados: false, pagina);
                return Ok(new { message = "Clientes obtidos com sucesso!", dados = clientes });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Recupera o histórico de todos os clientes da empresa, incluindo os deletados (Audit)
        [HttpGet("empresa/{empresaId}/historico")]
        public async Task<IActionResult> ObterHistoricoClientesPorEmpresa(long empresaId, [FromQuery] int pagina = 1)
        {
            try
            {
                var clientes = await _clienteService.ObterClientesPorEmpresa(empresaId, incluirDeletados: true, pagina);
                return Ok(new { message = "Histórico obtido com sucesso!", dados = clientes });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Busca os detalhes de um cliente específico pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterClientePorId(long id)
        {
            try
            {
                var cliente = await _clienteService.ObterClientePorId(id, incluirDeletados: false);
                return Ok(new { message = "Cliente obtido com sucesso!", dados = cliente });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Busca os detalhes de um cliente pelo ID, permitindo visualizar registros deletados
        [HttpGet("{id}/historico")]
        public async Task<IActionResult> ObterClientePorIdHistorico(long id)
        {
            try
            {
                var cliente = await _clienteService.ObterClientePorId(id, incluirDeletados: true);
                return Ok(new { message = "Cliente histórico obtido com sucesso!", dados = cliente });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion
    }
}