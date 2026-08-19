namespace BarberFlow.API.DTOs.HorarioProfissional
{
    public class HorarioProfissionalResponseDto
    {
        #region Identificação
        public long Id { get; set; }
        public long ProfissionalId { get; set; }
        public long EmpresaId { get; set; }
        public string NomeProfissional { get; set; } = string.Empty;
        #endregion

        #region Configuração de Horários
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
        public TimeOnly? HoraInicioAlmoco { get; set; }
        public TimeOnly? HoraFimAlmoco { get; set; }
        #endregion

        #region Status
        public bool Ativo { get; set; }
        #endregion

        #region Informações Auxiliares (Contexto)
        public List<DayOfWeek> DiasJaCadastrados { get; set; } = new();
        public List<RegraHorarioEmpresaDto> RegrasFuncionamento { get; set; } = new();
        #endregion
    }

    public class RegraHorarioEmpresaDto
    {
        #region Propriedades
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly? HoraAbertura { get; set; }
        public TimeOnly? HoraFechamento { get; set; }
        #endregion
    }
}