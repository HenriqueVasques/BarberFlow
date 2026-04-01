    using BarberFlow.API.Helpers;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    namespace BarberFlow.API.Models 
    {
        [Index(nameof(Slug), IsUnique = true)]
        [Index(nameof(CNPJ), IsUnique = true)]
        public class Empresa
        {
            #region Properties
            public long Id { get; set; }
            [Required, MaxLength(150)]
            public string Nome { get; set; }
            [Required, MaxLength(150)]
            public bool Ativo { get; set; } = true; 
            public bool IsDeleted { get; set; } = false;
            public string Slug { get; set; }
            public string CNPJ { get; set; }
            public DateTime DataCriacao { get; set; }
            public DateTime DataAtualizacao { get; set; }
            //nav prop
            public ICollection<Usuario> Usuarios { get; set; }
            public ICollection<Servico> Servicos { get; set; }
            public ICollection<HorarioFuncionamentoEmpresa> HorariosFuncionamentoEmpresa { get; set; }
            #endregion

            #region Constructors
            public Empresa(string nome, string cnpj)
            {
                Nome = nome;
                CNPJ = cnpj;
                DataCriacao = DateTime.UtcNow;
                DataAtualizacao = DateTime.UtcNow;
            }

            protected Empresa() {}
            #endregion
        }
    }
