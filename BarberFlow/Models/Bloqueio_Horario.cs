using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Bloqueio_Horario
    {
        public BigInteger Id { get; set; }
        public BigInteger EmpresaId { get; set; }
        public BigInteger ProfissionalId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public string Motivo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
