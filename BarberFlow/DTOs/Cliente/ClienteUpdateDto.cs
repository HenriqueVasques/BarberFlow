using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Cliente
{
    public class ClienteUpdateDto
    {
        #region Informações Pessoais
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;
        #endregion

        #region Contato
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O WhatsApp é obrigatório.")]
        public string Whatsapp { get; set; } = string.Empty;
        #endregion
    }
}