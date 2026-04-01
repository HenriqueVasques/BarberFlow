namespace BarberFlow.API.DTOs
{
    public class HorarioProfissionalResponseDto
    {
        public long Id { get; set; }
        public long ProfissionalId { get; set; }
        public long EmpresaId { get; set; }
        public string NomeProfissional { get; set; }    
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
        public TimeOnly HoraInicioAlmoco { get; set; }
        public TimeOnly HoraFimAlmoco { get; set; }
        public bool Ativo { get; set; }
        public bool IsDeleted { get; set; }

        public List<DayOfWeek> DiasJaCadastrados { get; set; }
        public List<RegraHorarioEmpresaDto> RegrasFuncionamento { get; set; }
    }
    public class RegraHorarioEmpresaDto
    {
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly? HoraAbertura { get; set; }
        public TimeOnly? HoraFechamento { get; set; }
    }
}
