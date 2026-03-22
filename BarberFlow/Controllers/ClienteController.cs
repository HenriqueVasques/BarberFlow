using BarberFlow.API.DTOs.Cliente;
using BarberFlow.API.Models;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromBody] ClienteCreateDto dto)
        {
            try
            {
                var cliente = await _clienteService.CriarCliente(dto);
                var response = MapearParaResponseDto(cliente);
                return StatusCode(201, new
                {
                    message = "Cliente criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCliente(long id, [FromBody] ClienteUpdateDto dto)
        {
            try
            {
                var cliente = await _clienteService.AtualizarCliente(id, dto);
                var response = MapearParaResponseDto(cliente);
                return Ok(new
                {
                    message = "Cliente atualizado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCliente(long id)
        {
            try
            {
                var cliente = await _clienteService.DeletarCliente(id);
                var response = MapearParaResponseDto(cliente);
                return Ok(new
                {
                    message = "Cliente deletado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{empresaId}Clientes-Por-Empresa")]
        public async Task<IActionResult> ObterClientesPorEmpresa(long empresaId)
        {
            try
            {
                var clientes = await _clienteService.ObterClientesPorEmpresa(empresaId);
                var response = clientes.Select(c => new ClienteResponseDto
                {
                    Id = c.Id,
                    Nome = c.Usuario.Nome,
                    Email = c.Usuario.Email,
                    Telefone = c.Telefone,
                    Whatsapp = c.Whatsapp,
                    EmpresaId = c.EmpresaId,
                    UsuarioId = c.UsuarioId,
                    DataCriacao = c.DataCriacao,
                    DataAtualizacao = c.DataAtualizacao,
                    IsDeleted = c.IsDeleted,
                    Ativo = c.Ativo
                });
                return Ok(new
                {
                    message = "Clientes obtidos com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}Clientes-Por-Id")]
        public async Task<IActionResult> ObterClientePorId(long id)
        {
            try
            {
                var cliente = await _clienteService.ObterClientePorId(id);
                var response = MapearParaResponseDto(cliente);
                return Ok(new
                {
                    message = "Cliente obtido com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private static ClienteResponseDto MapearParaResponseDto(Cliente cliente)
        {
            return new ClienteResponseDto
            {
                Id = cliente.Id,
                Nome = cliente.Usuario?.Nome ?? "N/A",
                Email = cliente.Usuario?.Email ?? "N/A",
                Telefone = cliente.Telefone,
                Whatsapp = cliente.Whatsapp,
                EmpresaId = cliente.EmpresaId,
                UsuarioId = cliente.UsuarioId,
                DataCriacao = cliente.DataCriacao,
                DataAtualizacao = cliente.DataAtualizacao,
                IsDeleted = cliente.IsDeleted,
                Ativo = cliente.Ativo
            };
        }
    }
}