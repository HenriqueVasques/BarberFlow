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

        [HttpPost("google")]
        public async Task<IActionResult> LoginComGoogle([FromBody] GoogleLoginDto dto)
        {
            try
            {
                var token = await _authService.LoginComGoogle(dto);
                return Ok(new { message = "Login realizado com sucesso!", token });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno ao processar a autenticação com o Google." });
            }
        }


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