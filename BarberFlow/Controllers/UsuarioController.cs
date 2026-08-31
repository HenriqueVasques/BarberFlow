using BarberFlow.API.DTOs.Usuario;
using BarberFlow.API.Interfaces.IServices;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        #region Comandos (Escrita)

        // Cadastra um novo usuário no sistema vinculado a uma empresa
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] UsuarioCreateDto dto)
        {
            try
            {
                var usuario = await _usuarioService.CriarUsuario(dto);
                return StatusCode(201, new { message = "Usuário criado com sucesso!", dados = usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Atualiza os dados cadastrais do usuário (nome, e-mail, telefone, whatsapp)
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(long id, [FromBody] UsuarioUpdateDto dto)
        {
            try
            {
                await _usuarioService.AtualizarUsuario(id, dto);
                return Ok(new { message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Realiza a remoção lógica (soft delete) e desativa o usuário
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarUsuario(long id)
        {
            try
            {
                await _usuarioService.DeletarUsuario(id);
                return Ok(new { message = "Usuário deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Altera e criptografa a nova senha de acesso do usuário
        [HttpPut("{id}/alterar-senha")]
        public async Task<IActionResult> AlterarSenha(long id, [FromBody] UsuarioAlterarSenhaDto dto)
        {
            try
            {
                await _usuarioService.AlterarSenha(id, dto);
                return Ok(new { message = "Senha alterada com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Consultas (Leitura)

        // Obtém os detalhes de um usuário específico pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            try
            {
                var usuario = await _usuarioService.ObterPorId(id);
                return Ok(new { message = "Usuário encontrado com sucesso!", dados = usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Lista todos os usuários cadastrados vinculados a uma determinada empresa
        [HttpGet("empresa/{empresaId}")]
        public async Task<IActionResult> ObterUsuarioPorEmpresa(long empresaId)
        {
            try
            {
                var usuarios = await _usuarioService.ObterUsuariosPorEmpresa(empresaId);
                return Ok(new { message = "Usuários encontrados com sucesso!", dados = usuarios });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion
    }
}