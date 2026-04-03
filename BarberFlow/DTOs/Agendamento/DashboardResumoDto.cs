namespace BarberFlow.API.DTOs.Agendamento
{
    public class DashboardResumoDto
    {
        #region Identificação do Período
        public DateOnly Data { get; set; }
        #endregion

        #region Indicadores de Desempenho (Métricas)
        public decimal FaturamentoTotal { get; set; }
        public int QuantidadeAtendimentos { get; set; }
        #endregion
    }
}