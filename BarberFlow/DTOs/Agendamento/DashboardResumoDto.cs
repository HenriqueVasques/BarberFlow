namespace BarberFlow.API.DTOs.Agendamento
{
    public class DashboardResumoDto
    {
        public DateOnly Data { get; set; }
        public decimal FaturamentoTotal { get; set; }
        public int QuantidadeAtendimentos { get; set; }

    }
}
