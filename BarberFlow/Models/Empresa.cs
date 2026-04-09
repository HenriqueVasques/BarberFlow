using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BarberFlow.API.Models
{
    public class Empresa
    {
        #region Propriedades de Persistência
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public string CNPJ { get; set; }
        public bool Ativo { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        #endregion

        #region Propriedades de Navegação
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Servico> Servicos { get; set; }
        public virtual ICollection<HorarioFuncionamentoEmpresa> HorariosFuncionamentoEmpresa { get; set; }
        #endregion

        #region Construtores
        public Empresa() { }
        #endregion
    }
}