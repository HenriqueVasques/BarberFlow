using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Profissional
{
    public class ProfissionalUpdateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail em formato válido.")]
        public string Email { get; set; } = string.Empty;

        public string? Telefone { get; set; }
        public string? Whatsapp { get; set; }

        [Range(0, 100, ErrorMessage = "O percentual de comissão deve estar entre 0 e 100.")]
        public decimal PercentualComissao { get; set; }
    }
}