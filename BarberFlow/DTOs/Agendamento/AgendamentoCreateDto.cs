using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Agendamento
{
    public class AgendamentoCreateDto
    {
        #region Identificadores (Chaves e Relacionamentos)
        [Required(ErrorMessage = "A empresa é obrigatória.")]
        public long? EmpresaId { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public long? ClienteId { get; set; }

        [Required(ErrorMessage = "O serviço do profissional é obrigatório.")]
        public long? ProfissionalServicoId { get; set; }
        #endregion

        #region Datas e Horários
        [Required(ErrorMessage = "A data e hora de início são obrigatórias.")]
        public DateTime? DataHoraInicio { get; set; }
        #endregion
    }
}