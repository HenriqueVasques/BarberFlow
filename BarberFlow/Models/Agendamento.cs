using BarberFlow.API.Enums;

namespace BarberFlow.API.Models
{
    public class Agendamento
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long ClienteId { get; set; }
        public long ProfissionalServicoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
        public int? FormaPagamento { get; set; }
        public decimal PrecoNoMomento { get; set; }
        #endregion

        #region Auditoria e Controle
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        #endregion

        #region Propriedades de Navegação (Relacionamentos)
        public virtual Empresa Empresa { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ProfissionalServico ProfissionalServico { get; set; }
        #endregion
    }
}