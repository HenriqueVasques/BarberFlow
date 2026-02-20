using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Cliente
    {
        public BigInteger Id { get; set; }
        public BigInteger EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Whatsapp { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }
}
