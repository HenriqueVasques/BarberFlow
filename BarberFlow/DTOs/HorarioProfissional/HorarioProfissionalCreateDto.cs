namespace BarberFlow.API.DTOs
{
    public class HorarioProfissionalCreateDto
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        public DayOfWeek DiaSemana { get; set; }

        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }


        public TimeOnly HoraInicioAlmoco { get; set; }
        public TimeOnly HoraFimAlmoco { get; set; }
    }
}
