using BarberFlow.API.Enums;

namespace BarberFlow.API.DTOs.Agendamento
{
    public class AgendamentoAgendaDiaDto
    {
        #region Informações de Exibição (Nomes)
        public string NomeCliente { get; set; } = string.Empty;
        public string NomeServico { get; set; } = string.Empty;
        #endregion

        #region Valores e Status
        public decimal Preco { get; set; }
        public StatusAgendamento Status { get; set; }
        public string StatusDescricao => Status.ToString();
        #endregion

        #region Horários
        public DateTime InicioDoDia { get; set; }
        public DateTime FimDoDia { get; set; }
        #endregion
    }
}