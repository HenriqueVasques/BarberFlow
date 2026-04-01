using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BarberFlow.API.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- AGENDAMENTO ---
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Empresa)
                .WithMany()
                .HasForeignKey(a => a.EmpresaId);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.ProfissionalServico)
                .WithMany()
                .HasForeignKey(a => a.ProfissionalServicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- BLOQUEIO HORÁRIO ---
            modelBuilder.Entity<BloqueioHorario>()
                .HasOne(bh => bh.Empresa)
                .WithMany()
                .HasForeignKey(bh => bh.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BloqueioHorario>()
                .HasOne(bh => bh.Profissional)
                .WithMany()
                .HasForeignKey(bh => bh.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- CLIENTE ---
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.EmpresaId);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- HORÁRIOS ---
            modelBuilder.Entity<HorarioFuncionamentoEmpresa>()
                .HasOne(hfe => hfe.Empresa)
                .WithMany()
                .HasForeignKey(hfe => hfe.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HorarioProfissional>()
                .HasOne(hp => hp.Empresa)
                .WithMany()
                .HasForeignKey(hp => hp.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HorarioProfissional>()
                .HasOne(hp => hp.Profissional)
                .WithMany()
                .HasForeignKey(hp => hp.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- PROFISSIONAL ---
            modelBuilder.Entity<Profissional>()
                .HasOne(p => p.Empresa)
                .WithMany()
                .HasForeignKey(p => p.EmpresaId);

            modelBuilder.Entity<Profissional>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId);

            // --- PROFISSIONAL SERVIÇO (M:N) ---
            modelBuilder.Entity<ProfissionalServico>()
                .HasOne(ps => ps.Profissional)
                .WithMany(p => p.ProfissionalServicos)
                .HasForeignKey(ps => ps.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProfissionalServico>()
                .HasOne(ps => ps.Servico)
                .WithMany()
                .HasForeignKey(ps => ps.ServicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- SERVICO ---
            modelBuilder.Entity<Servico>()
                .HasOne(s => s.Empresa)
                .WithMany(e => e.Servicos)
                .HasForeignKey(s => s.EmpresaId);

            // --- USUARIO ---
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Empresa)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---  HORARIO FUNCIONAMENTO EMPRESA  ---
            modelBuilder.Entity<HorarioFuncionamentoEmpresa>()
                .HasOne(hfe => hfe.Empresa)
                .WithMany(e => e.HorariosFuncionamentoEmpresa)
                .HasForeignKey(hfe => hfe.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- ÍNDICES ÚNICOS ---

            // Impede duplicar o mesmo serviço para o mesmo profissional
            modelBuilder.Entity<ProfissionalServico>()
                .HasIndex(ps => new { ps.ProfissionalId, ps.ServicoId })
                .IsUnique();

            // Impede a empresa de ter dois horários para o mesmo dia
            modelBuilder.Entity<HorarioFuncionamentoEmpresa>()
                .HasIndex(h => new { h.EmpresaId, h.DiaSemana })
                .IsUnique();

            // Impede o profissional de ter dois turnos no mesmo dia
            modelBuilder.Entity<HorarioProfissional>()
                .HasIndex(hp => new { hp.ProfissionalId, hp.DiaSemana })
                .IsUnique();

            // --- FILTROS GLOBAIS (SOFT DELETE) ---
            modelBuilder.Entity<Servico>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<Profissional>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Cliente>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Usuario>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<BloqueioHorario>().HasQueryFilter(bh => !bh.IsDeleted);

            // --- PRECISÃO DECIMAL ---
            modelBuilder.Entity<Servico>().Property(s => s.PrecoBase).HasPrecision(18, 2);
            modelBuilder.Entity<Agendamento>().Property(a => a.PrecoNoMomento).HasPrecision(18, 2);
            modelBuilder.Entity<Profissional>().Property(p => p.PercentualComissao).HasPrecision(5, 2);
            modelBuilder.Entity<ProfissionalServico>().Property(p => p.PrecoPersonalizado).HasPrecision(18, 2);

            // --- CONVERSOR UTC (POSTGRESQL COMPLIANCE) ---
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
        }
    }
}