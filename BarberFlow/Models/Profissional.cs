namespace BarberFlow.API.Models
{
    public class Profissional
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        public decimal PercentualComissao { get; set; }
        public bool Ativo { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Empresa Empresa { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<ProfissionalServico> ProfissionalServicos { get; set; }
        public ICollection<Agendamento> Agendamentos { get; set; }
        public ICollection<HorarioProfissional> HorariosProfissionais { get; set; }

        public Profissional()
        {
            ProfissionalServicos = new List<ProfissionalServico>();
            Agendamentos = new List<Agendamento>();
            HorariosProfissionais = new List<HorarioProfissional>();
        }

        public Profissional(long empresaId, long usuarioId, decimal percentualComissao)
        {
            EmpresaId = empresaId;
            UsuarioId = usuarioId;
            PercentualComissao = percentualComissao;
            Ativo = true;
            IsDeleted = false;
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;

            ProfissionalServicos = new List<ProfissionalServico>();
            Agendamentos = new List<Agendamento>();
            HorariosProfissionais = new List<HorarioProfissional>();
        }
    }
}
