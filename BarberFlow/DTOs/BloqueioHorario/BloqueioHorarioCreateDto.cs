using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.BloqueioHorario
{
    public class BloqueioHorarioCreateDto
    {
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataHoraInicio { get; set; }
        [Required(ErrorMessage = "A data do final é obrigatória.")]
        public DateTime DataHoraFim { get; set; }
        [Required(ErrorMessage = "O motivo é obrigatória.")]
        public string Motivo { get; set; }
    }
}