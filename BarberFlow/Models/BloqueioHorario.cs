using System.Numerics;

namespace BarberFlow.API.Models
{
    public class BloqueioHorario
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public string Motivo { get; set; }
        public DateTime DataCriacao { get; set; }

        //navegation properties
        public Empresa Empresa { get; set; }
        public Profissional Profissional { get; set; }
    }
}
