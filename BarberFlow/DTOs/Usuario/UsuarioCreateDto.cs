using System.ComponentModel.DataAnnotations;
using BarberFlow.API.Enums;

namespace BarberFlow.API.DTOs.Usuario
{
    public class UsuarioCreateDto
    {

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A empresa deve ser informada.")]
        public long EmpresaId { get; set; }
        public string Telefone { get; set; }
        public string Whatsapp { get; set; }
    }
}