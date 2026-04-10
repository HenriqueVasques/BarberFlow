namespace BarberFlow.API.DTOs
{
    public class HorarioFuncionamentoEmpresaResponseDto
    {
        #region Identificação
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        #endregion

        #region Regras de Negócio
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly? HoraAbertura { get; set; }
        public TimeOnly? HoraFechamento { get; set; }
        public bool EstaFechado { get; set; }
        #endregion

        #region Status
        public bool Ativo { get; set; }
        #endregion
    }
}