using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Auth
{
    public class GoogleLoginDto
    {
        [Required(ErrorMessage = "O token do Google é obrigatório.")]
        public string IdToken { get; set; } = string.Empty;
        public long? EmpresaId { get; set; }
    }
}