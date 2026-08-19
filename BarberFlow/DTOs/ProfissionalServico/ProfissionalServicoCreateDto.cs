using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.ProfissionalServico
{
    public class ProfissionalServicoCreateDto
    {
        [Required(ErrorMessage = "O ID do profissional é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "Informe um ID de profissional válido.")]
        public long ProfissionalId { get; set; }

        [Required(ErrorMessage = "O ID do serviço é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "Informe um ID de serviço válido.")]
        public long ServicoId { get; set; }

        [Range(0, 999999.99, ErrorMessage = "O preço personalizado não pode ser negativo.")]
        public decimal? PrecoPersonalizado { get; set; }

        [Range(1, 1440, ErrorMessage = "A duração personalizada deve ser de pelo menos 1 minuto.")]
        public int? DuracaoPersonalizadaMinutos { get; set; }
    }
}