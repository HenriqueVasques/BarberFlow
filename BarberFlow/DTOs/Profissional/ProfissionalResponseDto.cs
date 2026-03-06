namespace BarberFlow.API.DTOs.Profissional
{
    public class ProfissionalResponseDto
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        public decimal PercentualComissao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool IsDeleted { get; set; }
        public bool Ativo { get; set; }
    }
}
