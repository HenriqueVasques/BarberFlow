using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Usuario
{
    public class UsuarioUpdateDto
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        [Phone(ErrorMessage = "O número de telefone informado é inválido.")]
        public string? Telefone { get; set; }

        [Phone(ErrorMessage = "O número de WhatsApp informado é inválido.")]
        public string? Whatsapp { get; set; }
    }
}
