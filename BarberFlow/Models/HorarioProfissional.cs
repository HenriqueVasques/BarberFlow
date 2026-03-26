using BarberFlow.API.Models;

public class HorarioProfissional
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public long ProfissionalId { get; set; }
    public DayOfWeek DiaSemana { get; set; } 

    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFim { get; set; }


    public TimeOnly? HoraInicioAlmoco { get; set; }
    public TimeOnly? HoraFimAlmoco { get; set; }

    public bool Ativo { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    // Navigation properties
    public Empresa Empresa { get; set; }
    public Profissional Profissional { get; set; }
}