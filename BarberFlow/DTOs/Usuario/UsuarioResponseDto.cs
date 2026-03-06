using BarberFlow.API.Enums;

namespace BarberFlow.API.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public long EmpresaId { get; set; }
        public PerfilUsuario Perfil { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool IsDeleted { get; set; }
    }
}
