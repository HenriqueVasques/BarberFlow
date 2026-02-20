using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Profissional
    {
        public BigInteger Id { get; set; }
        public BigInteger EmpresaId { get; set; }
        public BigInteger UsuarioId { get; set; }
        public decimal PercentualComissao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
