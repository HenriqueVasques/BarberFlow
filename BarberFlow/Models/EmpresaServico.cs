namespace BarberFlow.API.Models
{
    public class EmpresaServico
    {
            public long Id { get; set; }

            public long EmpresaId { get; set; }
            public long ServicoId { get; set; }

            public decimal PrecoPadrao { get; set; }

            // Navigation properties
            public Empresa Empresa { get; set; }
            public Servico Servico { get; set; }
            // lista de profissionais que oferecem esse serviço na empresa
            public ICollection<ProfissionalServico> ProfissionaisServicos { get; set; }

    }
}
