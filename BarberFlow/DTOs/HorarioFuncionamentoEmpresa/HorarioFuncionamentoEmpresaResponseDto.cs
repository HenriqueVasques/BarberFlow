namespace BarberFlow.API.DTOs;
    public class HorarioFuncionamentoEmpresaResponseDto
    {
        public long Id { get; set; }
        public string NomeEmpresa { get; set; } 
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly? HoraAbertura { get; set; }
        public TimeOnly? HoraFechamento { get; set; }
        public bool EstaFechado { get; set; }

    }
