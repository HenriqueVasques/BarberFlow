using BarberFlow.API.Data.Context;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        #region Readonly Fields
        private readonly AppDbContext _appDbContext;
        #endregion

        #region Construtor
        public AgendamentoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Comandos (Escrita)
        public async Task Adicionar(Agendamento agendamento)
        {
            await _appDbContext.Agendamentos.AddAsync(agendamento);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Atualizar(Agendamento agendamento)
        {
            _appDbContext.Agendamentos.Update(agendamento);
            await _appDbContext.SaveChangesAsync();
        }
        #endregion

        #region Consultas (Leitura)

        public async Task<Agendamento?> ObterPorId(long id)
        {
            return await _appDbContext.Agendamentos
                .Include(a => a.Empresa)
                .Include(a => a.Servico)
                .Include(a => a.Profissional).ThenInclude(p => p.Usuario)
                .Include(a => a.Cliente).ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Agendamento>> ObterAgendaPorPeriodo(long profissionalId, long empresaId, DateTime inicio, DateTime fim, List<StatusAgendamento> statusFiltro)
        {
            return await _appDbContext.Agendamentos
                .AsNoTracking()
                .Include(a => a.Servico)
                .Include(a => a.Cliente).ThenInclude(c => c.Usuario)
                .Where(a => a.ProfissionalId == profissionalId &&
                            a.EmpresaId == empresaId &&
                            a.DataHoraInicio >= inicio &&
                            a.DataHoraInicio <= fim &&
                            statusFiltro.Contains(a.Status))
                .OrderBy(a => a.DataHoraInicio)
                .ToListAsync();
        }

        #endregion

        #region Validações de Conflito
        public async Task<bool> EstaOcupado(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null, long? bloqueioIdParaIgnorar = null)
        {
            return await TemConflitoAgendamento(profissionalId, inicio, fim, agendamentoIdParaIgnorar) ||
                   await TemConflitoBloqueio(profissionalId, inicio, fim, bloqueioIdParaIgnorar);
        }

        public async Task<bool> TemConflitoBloqueio(long profissionalId, DateTime inicio, DateTime fim, long? bloqueioIdParaIgnorar = null)
        {
            return await _appDbContext.Bloqueio_Horarios
            .AnyAsync(b =>
                b.ProfissionalId == profissionalId &&
                (bloqueioIdParaIgnorar == null || b.Id != bloqueioIdParaIgnorar) &&
                !b.IsDeleted &&
                inicio < b.DataHoraFim && fim > b.DataHoraInicio
            );
        }

        public async Task<bool> TemConflitoAgendamento(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null)
        {
            return await _appDbContext.Agendamentos.AnyAsync(a =>
                a.ProfissionalId == profissionalId &&
                (agendamentoIdParaIgnorar == null || a.Id != agendamentoIdParaIgnorar) &&
                a.Status != StatusAgendamento.Cancelado &&
                inicio < a.DataHoraFim && fim > a.DataHoraInicio
            );
        }
        #endregion
    }
}