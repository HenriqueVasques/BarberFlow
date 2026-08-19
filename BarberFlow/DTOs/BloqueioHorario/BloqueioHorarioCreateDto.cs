using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.BloqueioHorario
{
    public class BloqueioHorarioCreateDto
    {
        #region Identificadores (Chaves e Relacionamentos)
        [Required(ErrorMessage = "A empresa é obrigatória.")]
        public long? EmpresaId { get; set; }

        [Required(ErrorMessage = "O profissional é obrigatório.")]
        public long? ProfissionalId { get; set; }
        #endregion

        #region Datas e Horários
        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime? DataHoraInicio { get; set; }

        [Required(ErrorMessage = "A data final é obrigatória.")]
        public DateTime? DataHoraFim { get; set; }
        #endregion

        #region Informações Gerais
        [Required(ErrorMessage = "O motivo é obrigatório.")]
        [StringLength(200, ErrorMessage = "O motivo deve ter no máximo 200 caracteres.")]
        public string Motivo { get; set; } = string.Empty;
        #endregion
    }
}