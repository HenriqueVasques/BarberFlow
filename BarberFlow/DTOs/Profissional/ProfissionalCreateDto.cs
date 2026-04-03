using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Profissional
{
    public class ProfissionalCreateDto
    {
        public string Nome { get; set; }
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Whatsapp { get; set; }
        public string Senha { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        public decimal PercentualComissao { get; set; }
    }
}
