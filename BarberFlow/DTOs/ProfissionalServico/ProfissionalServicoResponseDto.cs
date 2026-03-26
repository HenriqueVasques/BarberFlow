namespace BarberFlow.API.DTOs
{
    public class ProfissionalServicoResponseDto
    {
        public string NomeServico { get; set; }
        public string NomeProfisional { get; set; }
        public long Id { get; set; }
        public long ProfissionalId { get; set; }
        public long ServicoId { get; set; }

        public decimal? PrecoPersonalizado { get; set; }
        public int? DuracaoPersonalizadaMinutos { get; set; }

        public bool Ativo { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
