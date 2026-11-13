using BarberFlow.API.DTOs.Auth;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IAuthService
    {
        #region Regras de Negócio (Autenticação)

        Task<LoginResponseDto> Login(LoginDto dto);
        Task<string> LoginComGoogle(GoogleLoginDto dto);

        #endregion
    }
}