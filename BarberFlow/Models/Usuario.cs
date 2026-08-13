using BarberFlow.API.Enums;

namespace BarberFlow.API.Models
{
    public class Usuario
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Whatsapp { get; set; }
        public string SenhaHash { get; set; }
        public PerfilUsuario Perfil { get; set; }
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
        public Usuario() { }

        
        #endregion
    }
}