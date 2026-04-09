namespace BarberFlow.API.Models
{
    public class Profissional
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        public decimal PercentualComissao { get; set; }
        public bool Ativo { get; set; } = true;
        #endregion

        #region Auditoria e Controle
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        #endregion

        #region Propriedades de Navegação (Relacionamentos)
        public virtual Empresa Empresa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<ProfissionalServico> ProfissionalServicos { get; set; }
        public virtual ICollection<Agendamento> Agendamentos { get; set; }
        public virtual ICollection<HorarioProfissional> HorariosProfissionais { get; set; }
        #endregion

        #region Construtores
        public Profissional()
        {
            ProfissionalServicos = new List<ProfissionalServico>();
            Agendamentos = new List<Agendamento>();
            HorariosProfissionais = new List<HorarioProfissional>();
        }

        public Profissional(long empresaId, long usuarioId, decimal percentualComissao)
        {
            EmpresaId = empresaId;
            UsuarioId = usuarioId;
            PercentualComissao = percentualComissao;
            Ativo = true;
            IsDeleted = false;
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;

            ProfissionalServicos = new List<ProfissionalServico>();
            Agendamentos = new List<Agendamento>();
            HorariosProfissionais = new List<HorarioProfissional>();
        }
        #endregion
    }
}