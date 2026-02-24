using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.Models
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Empresa
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Nome { get; set; }
        [Required, MaxLength(150)]
        public string Slug { get; set; }
        public string CNPJ { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
        public ICollection<Servico> Servicos { get; set; }
    }
}
