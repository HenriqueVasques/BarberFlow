namespace BarberFlow.API.DTOs.Auth
{
    public class LoginDto
    {
        #region Credenciais de Acesso
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        #endregion
    }
}