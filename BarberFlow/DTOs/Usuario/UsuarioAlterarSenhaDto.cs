using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.DTOs.Usuario
{
    public class UsuarioAlterarSenhaDto
    {
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        public required string Senha { get; set; }
    }
}
