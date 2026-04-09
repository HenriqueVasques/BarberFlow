namespace BarberFlow.API.Models
{
    public class BloqueioHorario
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public string Motivo { get; set; }
        #endregion

        #region Auditoria e Controle
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        #endregion

        #region Propriedades de Navegação (Relacionamentos)
        public virtual Empresa Empresa { get; set; }
        public virtual Profissional Profissional { get; set; }
        #endregion
    }
}