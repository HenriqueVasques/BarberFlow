namespace BarberFlow.API.Models
{
    public class Cliente
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }

        public bool Ativo { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        #endregion

        #region Privacidade e Consentimento
        public bool AceitouTermosPrivacidade { get; set; }
        public DateTime? DataConsentimento { get; set; }
        public string? IpConsentimento { get; set; }
        #endregion

        #region Propriedades de Navegação
        public virtual Empresa Empresa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Agendamento> Agendamentos { get; set; }
        #endregion
    }
}