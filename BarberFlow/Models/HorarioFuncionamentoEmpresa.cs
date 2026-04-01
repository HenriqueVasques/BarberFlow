using System.Numerics;

namespace BarberFlow.API.Models
{
    public class HorarioFuncionamentoEmpresa
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly? HoraAbertura { get; set; }
        public TimeOnly? HoraFechamento { get; set; }
        public bool EstaFechado { get; set; }
        public bool Ativo { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        //navegation properties
        public Empresa Empresa { get; set; }
    }
}
