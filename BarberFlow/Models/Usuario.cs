using System.Numerics;
using BarberFlow.API.Enums;

namespace BarberFlow.API.Models
{
    public class Usuario
    {
        private string senha;
        #region Properties
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public PerfilUsuario Perfil { get; set; }
        public bool Ativo { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        // Navigation properties
        public Empresa Empresa { get; set; }
        #endregion

        #region Constructors
        public Usuario(string nome, string email, string senhaHash, long empresaId, PerfilUsuario perfil)
        {
            EmpresaId = empresaId;
            Nome = nome;
            Email = email;
            Perfil = perfil;
            SenhaHash = senhaHash;
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;
        }

        protected Usuario() { }
        #endregion
    }
}
