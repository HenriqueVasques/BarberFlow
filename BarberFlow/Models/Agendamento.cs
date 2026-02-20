using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Agendamento
    {
        public BigInteger Id { get; set; }
        public BigInteger EmpresaId { get; set; }
        public BigInteger ClienteId { get; set; }
        public BigInteger ProfissionalId { get; set; }
        public BigInteger ServicoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public int Status { get; set; }
        public int FormaPagamento { get; set; }
        public decimal PrecoNoMomento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
