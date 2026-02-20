using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Servico
    {
        public BigInteger Id { get; set; }
        public BigInteger EmpresaId { get; set; }
        public string Nome { get; set; }
        public int DuracaoMinutos { get; set; }
        public Decimal PrecoBase { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
