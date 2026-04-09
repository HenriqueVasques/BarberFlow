namespace BarberFlow.API.Models
{
    public class ProfissionalServico
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public long ProfissionalId { get; set; }
        public long ServicoId { get; set; }

        public decimal? PrecoPersonalizado { get; set; }
        public int? DuracaoPersonalizadaMinutos { get; set; }

        public bool Ativo { get; set; } = true;
        #endregion

        #region Auditoria e Controle
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        #endregion

        #region Propriedades de Navegação (Relacionamentos)
        public virtual Profissional Profissional { get; set; }
        public virtual Servico Servico { get; set; }
        #endregion
    }
}