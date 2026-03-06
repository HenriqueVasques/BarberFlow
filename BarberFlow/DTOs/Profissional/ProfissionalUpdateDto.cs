namespace BarberFlow.API.DTOs.Profissional
{
    public class ProfissionalUpdateDto
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public string? Senha { get; set; }
        public decimal PercentualComissao { get; set; }
    }
}
