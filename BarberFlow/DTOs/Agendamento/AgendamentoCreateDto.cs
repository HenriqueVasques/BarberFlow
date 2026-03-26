namespace BarberFlow.API.DTOs.Agendamento
{
    public class AgendamentoCreateDto
    {
        public long EmpresaId { get; set; }
        public long ClienteId { get; set; }
        public long ProfissionalServicoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        //public int FormaPagamento { get; set; }
    }
}
