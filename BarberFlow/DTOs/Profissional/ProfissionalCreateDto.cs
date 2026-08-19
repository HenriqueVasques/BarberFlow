using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Profissional
{
    public class ProfissionalCreateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefone inválido.")]
        public string? Telefone { get; set; }

        [Phone(ErrorMessage = "WhatsApp inválido.")]
        public string? Whatsapp { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A empresa é obrigatória.")]
        [Range(1, long.MaxValue, ErrorMessage = "O ID da empresa deve ser um valor válido.")]
        public long EmpresaId { get; set; }

        public long UsuarioId { get; set; }

        [Range(0, 100, ErrorMessage = "O percentual de comissão deve estar entre 0 e 100.")]
        public decimal PercentualComissao { get; set; }
    }
}