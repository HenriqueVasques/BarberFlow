namespace BarberFlow.API.DTOs
{
    public class ProfissionalServicoCreateDto
    {
        public long ProfissionalId { get; set; }
        public long ServicoId { get; set; }

        public decimal? PrecoPersonalizado { get; set; }
        public int? DuracaoPersonalizadaMinutos { get; set; }
    }
}
