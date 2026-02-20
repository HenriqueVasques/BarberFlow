using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Profissional_Servico
    {
        public BigInteger Id { get; set; }
        public BigInteger EmpresaId { get; set; }
        public BigInteger ProfissionalId { get; set; }
        public BigInteger ServicoId { get; set; }
        public Decimal PrecoPersonalizado { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
