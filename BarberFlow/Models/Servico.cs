using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Servico
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public string Nome { get; set; }
        public int DuracaoMinutos { get; set; }
        public Decimal PrecoBase { get; set; }
        public bool Ativo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        //navegation properties
        public Empresa Empresa { get; set; }
    }
}
