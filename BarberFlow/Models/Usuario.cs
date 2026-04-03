
using BarberFlow.API.Enums;

namespace BarberFlow.API.Models
{
    public class Usuario
    {
        #region Properties
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Whatsapp { get; set; }
        public string SenhaHash { get; set; }
        public PerfilUsuario Perfil { get; set; }
        public bool Ativo { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Empresa Empresa { get; set; }
        #endregion

        #region Constructors
        public Usuario(string nome, string email, string telefone, string whatsapp, string senhaHash, long empresaId, PerfilUsuario perfil)
        {
            EmpresaId = empresaId;
            Nome = nome;
            Email = email;
            Telefone = telefone;
            Whatsapp = whatsapp;
            Perfil = perfil;
            SenhaHash = senhaHash;
            Ativo = true;
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;
        }

        public Usuario() { }
        #endregion
    }
}
