using BarberFlow.API.Models;

public class ProfissionalServico
{
    public long Id { get; set; }
    public long ProfissionalId { get; set; }
    public long ServicoId { get; set; }

    public decimal? PrecoPersonalizado { get; set; } 
    public int? DuracaoPersonalizadaMinutos { get; set; } 

    public bool Ativo { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Profissional Profissional { get; set; }
    public Servico Servico { get; set; }
}