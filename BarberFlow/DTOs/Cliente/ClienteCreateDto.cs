using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Cliente
{
    public class ClienteCreateDto
    {
        #region Identificadores
        [Required(ErrorMessage = "A empresa é obrigatória.")]
        public long? EmpresaId { get; set; }
        #endregion

        #region Informações Pessoais
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string Senha { get; set; } = string.Empty;
        #endregion

        #region Contato
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O WhatsApp é obrigatório.")]
        public string Whatsapp { get; set; } = string.Empty;
        #endregion

        #region Termos de Privacidade e LGPD
        [Range(typeof(bool), "true", "true", ErrorMessage = "Você precisa aceitar os Termos de Privacidade para se cadastrar.")]
        public bool AceitouTermosPrivacidade { get; set; }
        #endregion
    }
}