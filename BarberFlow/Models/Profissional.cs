using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Profissional
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        public decimal PercentualComissao { get; set; }
        public bool Ativo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        // Navigation properties
        public Empresa Empresa { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<ProfissionalServico> ProfissionalServicos { get; set; }
        public ICollection<Agendamento> Agendamentos { get; set; }
    }
}
