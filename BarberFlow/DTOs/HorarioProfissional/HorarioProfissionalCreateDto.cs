using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.HorarioProfissional
{
    public class HorarioProfissionalCreateDto
    {
        #region Chaves Estrangeiras
        [Required(ErrorMessage = "O ID do profissional é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID do profissional inválido.")]
        public long? ProfissionalId { get; set; }
        #endregion

        #region Dia da Semana
        [Required(ErrorMessage = "O dia da semana é obrigatório.")]
        [EnumDataType(typeof(DayOfWeek), ErrorMessage = "Dia da semana inválido.")]
        public DayOfWeek? DiaSemana { get; set; }
        #endregion

        #region Expediente de Trabalho
        [Required(ErrorMessage = "A hora de início do expediente é obrigatória.")]
        public TimeOnly? HoraInicio { get; set; }

        [Required(ErrorMessage = "A hora de fim do expediente é obrigatória.")]
        public TimeOnly? HoraFim { get; set; }
        #endregion

        #region Intervalo / Almoço (Opcional)
        public TimeOnly? HoraInicioAlmoco { get; set; }
        public TimeOnly? HoraFimAlmoco { get; set; }
        #endregion
    }
}