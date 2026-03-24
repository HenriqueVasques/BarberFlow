namespace BarberFlow.API.DTOs;

public class HorarioFuncionamentoEmpresaUpadteDto
{
    public DayOfWeek DiaSemana { get; set; }
    public TimeSpan? HoraAbertura { get; set; }
    public TimeSpan? HoraFechamento { get; set; }
    public bool EstaFechado { get; set; }
}
