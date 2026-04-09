namespace BarberFlow.API.Models
{
    public class HorarioProfissional
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        public DayOfWeek DiaSemana { get; set; }

        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }

        public TimeOnly HoraInicioAlmoco { get; set; }
        public TimeOnly HoraFimAlmoco { get; set; }

        public bool Ativo { get; set; } = true;
        #endregion

        #region Auditoria e Controle
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        #endregion

        #region Propriedades de Navegação (Relacionamentos)
        public virtual Empresa Empresa { get; set; }
        public virtual Profissional Profissional { get; set; }
        #endregion
    }
}