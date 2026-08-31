using BarberFlow.API.DTOs.Auth;
using BarberFlow.API.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BarberFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Atributos e Construtor

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #endregion

        #region Endpoints HTTP (Autenticação)

        // Realiza o login do usuário no sistema e retorna o token JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var login = await _authService.Login(dto);
                return Ok(new { message = "Login realizado com sucesso!", dados = login});
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        #endregion
    }
}