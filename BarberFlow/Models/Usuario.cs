using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Usuario
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public int Perfil { get; set; }
        public bool Ativo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        // Navigation properties
        public Empresa Empresa { get; set; }
    }
}
