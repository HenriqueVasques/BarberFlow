namespace BarberFlow.API.DTOs.Auth
{
    public class LoginDto
    {
        #region Credenciais de Acesso
        public required string Email { get; set; } = string.Empty;
        public required string Senha { get; set; } = string.Empty;
        #endregion
    }
}