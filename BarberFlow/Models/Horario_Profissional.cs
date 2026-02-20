using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Horario_Profissional
    {
        public BigInteger Id { get; set; }
        public BigInteger EmpresaId { get; set; }
        public BigInteger ProfissionalId { get; set; }
        public int DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
    }
}
