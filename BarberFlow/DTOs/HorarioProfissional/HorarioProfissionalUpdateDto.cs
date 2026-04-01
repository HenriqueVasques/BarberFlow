namespace BarberFlow.API.DTOs
{
    public class HorarioProfissionalUpdateDto
    {
        required
        public DayOfWeek? DiaSemana { get; set; }
        required
        public TimeOnly? HoraInicio { get; set; }
        required
        public TimeOnly? HoraFim { get; set; }

        required
        public TimeOnly? HoraInicioAlmoco { get; set; }
        required
        public TimeOnly? HoraFimAlmoco { get; set; }
    }
}
