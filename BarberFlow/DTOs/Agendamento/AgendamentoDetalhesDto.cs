using BarberFlow.API.Enums;

public class AgendamentoDetalhesDto
{
    public long Id { get; set; }
    public long ProfissionalId { get; set; }
    public long ClienteId { get; set; }
    public long ServicoId { get; set; }

    public string NomeProfissional { get; set; }
    public string NomeCliente { get; set; }
    public string NomeServico { get; set; }
    public string NomeEmpresa { get; set; }

    // Valores e Tempos
    public decimal PrecoNoMomento { get; set; }
    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }

    // Status (Enum)
    public StatusAgendamento Status { get; set; }
    public string StatusDescricao => Status.ToString(); 
}