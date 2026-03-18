using BarberFlow.API.DTOs.Usuario;
using BarberFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        #region private fields
        private readonly UsuarioService _usuarioService;
        
        #endregion
        #region constructors
        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        #endregion

        #region public methods
        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioCreateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados do usuário são obrigatórios.");
                }

                var usuario = await _usuarioService.CriarUsuario(dto);
                if (usuario == null)
                {
                    return BadRequest("Não foi possível criar o usuário.");
                }

                var response = new UsuarioResponseDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    EmpresaId = usuario.EmpresaId,
                    Perfil = usuario.Perfil,
                    DataCriacao = usuario.DataCriacao,
                    DataAtualizacao = usuario.DataAtualizacao,
                    IsDeleted = usuario.IsDeleted
                };

                return StatusCode(201, new
                {
                    message = "Usuário criado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario(long id, UsuarioUpdateDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados do usuário são obrigatórios.");
                }
                var usuario = await _usuarioService.AtualizarUsuario(id, dto);
                if (usuario == null)
                {
                    return NotFound($"Usuário com id {id} não encontrado.");
                }
                var response = new UsuarioResponseDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    EmpresaId = usuario.EmpresaId,
                    Perfil = usuario.Perfil,
                    DataCriacao = usuario.DataCriacao,
                    DataAtualizacao = usuario.DataAtualizacao,
                    IsDeleted = usuario.IsDeleted
                };
                return StatusCode(201, new
                {
                    message = "Usuário atualizado com sucesso!",
                    dados = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarUsuario(long id)
        {
            try
            {
                var usuario = await _usuarioService.DeletarUsuario(id);
                if (usuario == null)
                {
                    return NotFound($"Usuário com id {id} não encontrado.");
                }

                var response = new UsuarioResponseDto
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    EmpresaId = usuario.EmpresaId,
                    Perfil = usuario.Perfil,
                    DataCriacao = usuario.DataCriacao,
                    DataAtualizacao = usuario.DataAtualizacao,
                    IsDeleted = usuario.IsDeleted
                };

                return StatusCode(201, new
                {
                    message = "Empresa encontrada com sucesso!",
                    dados = response
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}/alterar-senha")]
        public async Task<IActionResult> AlterarSenha(long id, UsuarioAlterarSenhaDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Senha))
                {
                    return BadRequest("A nova senha é obrigatória.");
                }
               await _usuarioService.AlterarSenha(id, dto);

                return StatusCode(200, new
                {
                    message = "Senha alterada com sucesso!",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion
    }
}
