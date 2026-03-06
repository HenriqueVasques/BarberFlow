using BarberFlow.API.Models;

namespace BarberFlow.API.DTOs.Profissional
{
    public class ProfissionalCreateDto
    {
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        public decimal PercentualComissao { get; set; }
    }
}
