using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.HorarioFuncionamentoEmpresa
{
    public class HorarioFuncionamentoEmpresaCreateDto
    {
        #region Dia da Semana
        [Required(ErrorMessage = "O dia da semana é obrigatório.")]
        [EnumDataType(typeof(DayOfWeek), ErrorMessage = "Dia da semana inválido.")]
        public DayOfWeek? DiaSemana { get; set; }
        #endregion

        #region Horários e Funcionamento
        public TimeOnly? HoraAbertura { get; set; }
        public TimeOnly? HoraFechamento { get; set; }
        public bool EstaFechado { get; set; } = false;
        #endregion
    }
}