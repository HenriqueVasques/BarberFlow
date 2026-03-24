namespace BarberFlow.API.DTOs;
    public class HorarioFuncionamentoEmpresaResponseDto
    {
        public long Id { get; set; }
        public string NomeEmpresa { get; set; } 
        public DayOfWeek DiaSemana { get; set; }
        public TimeSpan? HoraAbertura { get; set; }
        public TimeSpan? HoraFechamento { get; set; }
        public bool EstaFechado { get; set; }

    }
