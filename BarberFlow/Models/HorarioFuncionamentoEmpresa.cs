using System.Numerics;

namespace BarberFlow.API.Models
{
    public class HorarioFuncionamentoEmpresa
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public int DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }

        //navegation properties
        public Empresa Empresa { get; set; }
    }
}
