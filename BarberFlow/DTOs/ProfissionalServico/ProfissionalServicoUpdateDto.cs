using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.ProfissionalServico
{
    public class ProfissionalServicoUpdateDto
    {
        [Range(0, 999999.99, ErrorMessage = "O preço personalizado não pode ser negativo.")]
        public decimal? PrecoPersonalizado { get; set; }

        [Range(1, 1440, ErrorMessage = "A duração personalizada deve ser de pelo menos 1 minuto.")]
        public int? DuracaoPersonalizadaMinutos { get; set; }
    }
}