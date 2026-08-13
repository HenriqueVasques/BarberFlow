using BarberFlow.API.Enums;

namespace BarberFlow.API.DTOs.Auth
{
    public class LoginResponseDto
    {
        #region Autenticação e Token
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
        #endregion

        #region Identificadores (Chaves e Relacionamentos)
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        #endregion

        #region Informações de Exibição
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public PerfilUsuario Perfil { get; set; }
        #endregion
    }
}