using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Usuario
{
    public class UsuarioUpdateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
        public required string Email { get; set; }

        [Phone(ErrorMessage = "O número de telefone informado é inválido.")]
        public string? Telefone { get; set; }

        [Phone(ErrorMessage = "O número de WhatsApp informado é inválido.")]
        public string? Whatsapp { get; set; }
    }
}