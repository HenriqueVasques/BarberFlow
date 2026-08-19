using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Servico
{
    public class ServicoUpdateDto
    {
        #region Properties

        [Required(ErrorMessage = "O nome do serviço é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome do serviço deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A duração do serviço é obrigatória.")]
        [Range(1, 1440, ErrorMessage = "A duração deve ser de no mínimo 1 minuto.")]
        public int DuracaoMinutos { get; set; }

        [Required(ErrorMessage = "O preço base é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "O preço base deve ser maior que zero.")]
        public decimal PrecoBase { get; set; }

        public bool Ativo { get; set; }

        #endregion
    }
}