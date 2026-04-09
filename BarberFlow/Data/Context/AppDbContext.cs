using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BarberFlow.API.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        #region DbSets
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<BloqueioHorario> BloqueioHorarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<HorarioFuncionamentoEmpresa> HorarioFuncionamentoEmpresas { get; set; }
        public DbSet<HorarioProfissional> HorarioProfissionais { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<ProfissionalServico> ProfissionalServicos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Configurações de Entidades (Fluent API)

            // EMPRESA
            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Slug).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CNPJ).IsRequired().HasMaxLength(14);
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.HasIndex(e => e.CNPJ).IsUnique();
            });

            // USUARIO
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Nome).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(180);
                entity.Property(u => u.Telefone).HasMaxLength(20);
                entity.Property(u => u.Whatsapp).HasMaxLength(20);
                entity.Property(u => u.SenhaHash).IsRequired().HasMaxLength(512);

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Nome).HasDatabaseName("IX_Usuario_Nome");

                entity.HasOne(u => u.Empresa)
                      .WithMany(e => e.Usuarios)
                      .HasForeignKey(u => u.EmpresaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // AGENDAMENTO
            modelBuilder.Entity<Agendamento>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.PrecoNoMomento).HasPrecision(18, 2);
                entity.HasOne(a => a.Empresa).WithMany().HasForeignKey(a => a.EmpresaId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.Cliente).WithMany(c => c.Agendamentos).HasForeignKey(a => a.ClienteId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.ProfissionalServico).WithMany().HasForeignKey(a => a.ProfissionalServicoId).OnDelete(DeleteBehavior.Restrict);
            });

            // BLOQUEIO HORÁRIO
            modelBuilder.Entity<BloqueioHorario>(entity =>
            {
                entity.HasKey(bh => bh.Id);
                entity.Property(bh => bh.Motivo).HasMaxLength(255);
                entity.HasOne(bh => bh.Empresa).WithMany().HasForeignKey(bh => bh.EmpresaId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(bh => bh.Profissional).WithMany().HasForeignKey(bh => bh.ProfissionalId).OnDelete(DeleteBehavior.Cascade);
            });

            // CLIENTE
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.IpConsentimento).HasMaxLength(45);
                entity.HasOne(c => c.Empresa).WithMany().HasForeignKey(c => c.EmpresaId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Usuario).WithMany().HasForeignKey(c => c.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            });

            // HORÁRIO FUNCIONAMENTO EMPRESA
            modelBuilder.Entity<HorarioFuncionamentoEmpresa>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.HasIndex(h => new { h.EmpresaId, h.DiaSemana }).IsUnique();
                entity.HasOne(h => h.Empresa).WithMany(e => e.HorariosFuncionamentoEmpresa).HasForeignKey(h => h.EmpresaId).OnDelete(DeleteBehavior.Cascade);
            });

            // HORÁRIO PROFISSIONAL
            modelBuilder.Entity<HorarioProfissional>(entity =>
            {
                entity.HasKey(hp => hp.Id);
                entity.HasIndex(hp => new { hp.ProfissionalId, hp.DiaSemana }).IsUnique();
                entity.HasOne(hp => hp.Empresa).WithMany().HasForeignKey(hp => hp.EmpresaId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(hp => hp.Profissional).WithMany().HasForeignKey(hp => hp.ProfissionalId).OnDelete(DeleteBehavior.Cascade);
            });

            // PROFISSIONAL
            modelBuilder.Entity<Profissional>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.PercentualComissao).HasPrecision(5, 2);
                entity.HasOne(p => p.Empresa).WithMany().HasForeignKey(p => p.EmpresaId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Usuario).WithMany().HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            });

            // PROFISSIONAL SERVIÇO
            modelBuilder.Entity<ProfissionalServico>(entity =>
            {
                entity.HasKey(ps => ps.Id);
                entity.Property(ps => ps.PrecoPersonalizado).HasPrecision(18, 2);
                entity.HasIndex(ps => new { ps.ProfissionalId, ps.ServicoId }).IsUnique();
                entity.HasOne(ps => ps.Profissional).WithMany(p => p.ProfissionalServicos).HasForeignKey(ps => ps.ProfissionalId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ps => ps.Servico).WithMany().HasForeignKey(ps => ps.ServicoId).OnDelete(DeleteBehavior.Cascade);
            });

            // SERVICO
            modelBuilder.Entity<Servico>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Nome).IsRequired().HasMaxLength(100);
                entity.Property(s => s.PrecoBase).HasPrecision(18, 2);
                entity.HasOne(s => s.Empresa).WithMany(e => e.Servicos).HasForeignKey(s => s.EmpresaId).OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region Filtros Globais (Soft Delete)
            modelBuilder.Entity<Empresa>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Servico>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<Profissional>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Cliente>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Usuario>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<BloqueioHorario>().HasQueryFilter(bh => !bh.IsDeleted);
            modelBuilder.Entity<HorarioProfissional>().HasQueryFilter(hp => !hp.IsDeleted);
            modelBuilder.Entity<ProfissionalServico>().HasQueryFilter(ps => !ps.IsDeleted);
            #endregion

            #region Conversores de Valor (UTC)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                        v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    ));
                }
            }
            #endregion
        }
    }
}