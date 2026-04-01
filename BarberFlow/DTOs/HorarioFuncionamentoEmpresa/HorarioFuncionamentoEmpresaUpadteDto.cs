namespace BarberFlow.API.DTOs;

public class HorarioFuncionamentoEmpresaUpadteDto
{
    public DayOfWeek DiaSemana { get; set; }
    public TimeOnly? HoraAbertura { get; set; }
    public TimeOnly? HoraFechamento { get; set; }
    public bool EstaFechado { get; set; }
}
