using System.Numerics;

namespace BarberFlow.API.Models
{
    public class ProfissionalServico
    {
        public long Id { get; set; }
        public long ProfissionalId { get; set; }
        public long EmpresaServicoId { get; set; }
        public decimal? PrecoPersonalizado { get; set; }
        public DateTime DataCriacao { get; set; }

        // Navigation properties
        public Profissional Profissional { get; set; }
        public EmpresaServico EmpresaServico { get; set; }
    }
}
