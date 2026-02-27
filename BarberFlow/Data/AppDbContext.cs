using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions ) : base(dbContextOptions) { }

        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<BloqueioHorario> Bloqueio_Horarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<EmpresaServico> EmpresaServico { get; set; }
        public DbSet<HorarioFuncionamentoEmpresa> HorarioFuncionamentoEmpresas { get; set; }
        public DbSet<HorarioProfissional> HorarioProfissionais { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<ProfissionalServico> ProfissionalServicos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Agendamento → Empresa
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Empresa)
                .WithMany()
                .HasForeignKey(a => a.EmpresaId);

            //Agendamento → Cliente
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            //Agendamento → Profissional
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Profissional)
                .WithMany(p => p.Agendamentos)
                .HasForeignKey(a => a.ProfissionalId)
                .OnDelete(DeleteBehavior.Restrict);

            //Agendamento → Servico
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Servico)
                .WithMany()
                .HasForeignKey(a => a.ServicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // BloqueioHorario → Empresa
            modelBuilder.Entity<BloqueioHorario>()
                .HasOne(bh => bh.Empresa)
                .WithMany()
                .HasForeignKey(bh => bh.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            // BloqueioHorario → Profissional
            modelBuilder.Entity<BloqueioHorario>()
                .HasOne(bh => bh.Profissional)
                .WithMany()
                .HasForeignKey(bh => bh.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cliente → Empresa
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.EmpresaId);

            // EmpresaServico → Empresa
            modelBuilder.Entity<EmpresaServico>()
                .HasOne(es => es.Empresa)
                .WithMany()
                .HasForeignKey(es => es.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            // EmpresaServico → Servico
            modelBuilder.Entity<EmpresaServico>()
                .HasOne(es => es.Servico)
                .WithMany()
                .HasForeignKey(es => es.ServicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // HorarioFuncionamentoEmpresa → Empresa
            modelBuilder.Entity<HorarioFuncionamentoEmpresa>()
                .HasOne(hfe => hfe.Empresa)
                .WithMany()
                .HasForeignKey(hfe => hfe.EmpresaId);

            // HorarioProfissional → Empresa
            modelBuilder.Entity<HorarioProfissional>()
                .HasOne(hp => hp.Empresa)
                .WithMany()
                .HasForeignKey(hp => hp.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            // HorarioProfissional → Profissional
            modelBuilder.Entity<HorarioProfissional>()
                .HasOne(hp => hp.Profissional)
                .WithMany()
                .HasForeignKey(hp => hp.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Profissional → Empresa
            modelBuilder.Entity<Profissional>()
                .HasOne(p => p.Empresa)
                .WithMany()
                .HasForeignKey(p => p.EmpresaId);

            // Profissional → Usuario
            modelBuilder.Entity<Profissional>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId);

            // ProfissionalServico → Profissional
            modelBuilder.Entity<ProfissionalServico>()
            .HasOne(ps => ps.Profissional)
            .WithMany(p => p.ProfissionalServicos) 
            .HasForeignKey(ps => ps.ProfissionalId)
            .OnDelete(DeleteBehavior.Cascade);

            // ProfissionalServico → EmpresaServico
            modelBuilder.Entity<ProfissionalServico>()
                .HasOne(ps => ps.EmpresaServico)
                .WithMany(es => es.ProfissionaisServicos)
                .HasForeignKey(ps => ps.EmpresaServicoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Servico → Empresa
            modelBuilder.Entity<Servico>()
                .HasOne(s => s.Empresa)
                .WithMany(e => e.Servicos)
                .HasForeignKey(s => s.EmpresaId);

            // Usuario → Empresa
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Empresa)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice único para evitar duplicidade
            modelBuilder.Entity<ProfissionalServico>()
                .HasIndex(ps => new { ps.ProfissionalId, ps.EmpresaServicoId })
                .IsUnique();

            modelBuilder.Entity<EmpresaServico>()
                .HasIndex(es => new { es.EmpresaId, es.ServicoId })
                .IsUnique();

            // 2. Travas de segurança para Dias da Semana (Índices Únicos)
            // Impede a empresa de ter dois "domingos", por exemplo.
            modelBuilder.Entity<HorarioFuncionamentoEmpresa>()
                .HasIndex(h => new { h.EmpresaId, h.DiaSemana })
                .IsUnique();

            // Impede o profissional de ter duas "terças-feiras" cadastradas.
            modelBuilder.Entity<HorarioProfissional>()
                .HasIndex(hp => new { hp.ProfissionalId, hp.DiaSemana })
                .IsUnique();

            base.OnModelCreating(modelBuilder);

            // Filtros para ignorar inativos automaticamente
            modelBuilder.Entity<Servico>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<Profissional>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Cliente>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Usuario>().HasQueryFilter(u => !u.IsDeleted);

            // Configurações de precisão para campos decimais
            modelBuilder.Entity<Servico>().Property(s => s.PrecoBase).HasPrecision(18, 2);
            modelBuilder.Entity<Agendamento>().Property(a => a.PrecoNoMomento).HasPrecision(18, 2);
            modelBuilder.Entity<Profissional>().Property(p => p.PercentualComissao).HasPrecision(5, 2);
            modelBuilder.Entity<ProfissionalServico>().Property(p => p.PrecoPersonalizado).HasPrecision(18, 2);
        }
    }
}
