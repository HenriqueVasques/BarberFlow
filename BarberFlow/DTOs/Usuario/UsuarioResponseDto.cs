using BarberFlow.API.Enums;

namespace BarberFlow.API.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        #region Identificação e Vínculo
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public PerfilUsuario Perfil { get; set; }
        #endregion

        #region Dados Cadastrais e Contato
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Whatsapp { get; set; }
        #endregion

        #region Status e Auditoria
        public bool Ativo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        #endregion
    }
}