namespace BarberFlow.API.Models
{
    public class Servico
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public string Nome { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal PrecoBase { get; set; }
        public bool Ativo { get; set; } = true;
        #endregion

        #region Auditoria e Controle
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        #endregion

        #region Propriedades de Navegação (Relacionamentos)
        public virtual Empresa Empresa { get; set; }
        #endregion

        #region Construtores
        public Servico() { }

        public Servico(string nome, int duracaoMinutos, decimal precoBase, long empresaId)
        {
            EmpresaId = empresaId;
            Nome = nome;
            DuracaoMinutos = duracaoMinutos;
            PrecoBase = precoBase;
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;
            Ativo = true;
            IsDeleted = false;
        }
        #endregion
    }
}