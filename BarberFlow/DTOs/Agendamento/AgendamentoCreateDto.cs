namespace BarberFlow.API.DTOs.Agendamento
{
    public class AgendamentoCreateDto
    {
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        public long ClienteId { get; set; }
        public long ServicoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        //public int FormaPagamento { get; set; }
    }
}
