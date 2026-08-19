using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Empresa
{
    public class EmpresaCreateDto
    {
        #region Informações Gerais
        [Required(ErrorMessage = "O nome da empresa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da empresa deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(18, ErrorMessage = "O CNPJ deve ter no máximo 18 caracteres.")]
        public string CNPJ { get; set; } = string.Empty;
        #endregion
    }
}