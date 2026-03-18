using BarberFlow.API.Enums;

namespace BarberFlow.API.DTOs.Agendamento
{
    public class AgendamentoAgendaDiaDto
    {
        public string NomeCliente { get; set; }
        public string NomeServico { get; set; }
        public DateTime InicioDoDia { get; set; }
        public DateTime FimDoDia { get; set; }
        public StatusAgendamento Status { get; set; }
        public decimal Preco { get; set; }
    }
}