using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Usuario
{
    public class UsuarioCreateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A empresa deve ser informada.")]
        [Range(1, long.MaxValue, ErrorMessage = "Informe um ID de empresa válido.")]
        public long EmpresaId { get; set; }

        // Contatos opcionais (nulos nativamente)
        [Phone(ErrorMessage = "Telefone em formato inválido.")]
        public string? Telefone { get; set; }

        [Phone(ErrorMessage = "WhatsApp em formato inválido.")]
        public string? Whatsapp { get; set; }
    }
}