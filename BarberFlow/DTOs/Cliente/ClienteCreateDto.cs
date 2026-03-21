using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Cliente
{
    public class ClienteCreateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Nome { get; set; }
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public required string Senha { get; set; }
        public long EmpresaId { get; set; }
        [Required(ErrorMessage = "O Telefone é obrigatório.")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "O Whatsapp é obrigatório.")]
        public string Whatsapp { get; set; }
        //public bool AceitouTermosPrivacidade { get; set; } = false; 
    }
}
