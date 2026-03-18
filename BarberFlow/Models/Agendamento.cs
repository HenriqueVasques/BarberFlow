using BarberFlow.API.Enums;
using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Agendamento
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long ClienteId { get; set; }
        public long ProfissionalId { get; set; }
        public long ServicoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
        public int ?FormaPagamento { get; set; }
        public decimal PrecoNoMomento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        //navegation properties
        public Empresa Empresa { get; set; }
        public Cliente Cliente { get; set; }
        public Profissional Profissional { get; set; }
        public Servico Servico { get; set; }


    }
}
