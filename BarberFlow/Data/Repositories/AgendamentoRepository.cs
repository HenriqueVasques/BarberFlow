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

        #region Public Methods
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

        public async Task<IEnumerable<Agendamento>> ObterPorEmpresaId(long empresaId)//ver apenas os agendamento pendentes e confirmados, os cancelados e finallizados precisam de outro método
        {
            return await _appDbContext.Agendamentos
                .Where(a => a.EmpresaId == empresaId && a.Status == StatusAgendamento.Confirmado || a.Status == StatusAgendamento.Pendente)
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Agendamento>> ObterPorProfissionalId(long profissionalId)//ver apenas os agendamento pendentes e confirmados, os cancelados e finallizados precisam de outro método
        {
            return await _appDbContext.Agendamentos
                .Where(a => a.ProfissionalId == profissionalId && a.Status == StatusAgendamento.Confirmado || a.Status == StatusAgendamento.Pendente)
                .Include(a => a.Cliente)
                .Include(a => a.Servico)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Agendamento?> ObterPorId(long id)//trazer o agendamento mesmo que esteja cancelado ou finalizado, para fins de histórico, tratar isso na camada de serviço
        {
            return await _appDbContext.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Profissional)
                .Include(a => a.Servico)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> EstaOcupado(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null, long? bloqueioIdParaIgnorar = null)
        {
            return await TemConflitoAgendamento(profissionalId, inicio, fim, agendamentoIdParaIgnorar) ||
                   await TemConflitoBloqueio(profissionalId, inicio, fim, bloqueioIdParaIgnorar);
        }

        public async Task<List<Agendamento>> ObterAgendaProfissionalPorData(long profissionalId, long empresaId, DateTime data)
        {
            var inicioDoDia = data.Date;
            var fimDoDia = data.Date.AddDays(1).AddTicks(-1);
            return await _appDbContext.Agendamentos
                .Include(a => a.Servico)
                .Include(a => a.Cliente)
                .Where(a => a.ProfissionalId == profissionalId &&
                            a.EmpresaId == empresaId &&
                            a.DataHoraInicio >= inicioDoDia &&
                            a.DataHoraInicio <= fimDoDia &&
                            a.Status != StatusAgendamento.Cancelado)
                .OrderBy(a => a.DataHoraInicio)
                .ToListAsync();
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
                inicio < a.DataHoraFim && fim > a.DataHoraInicio);
        }

        #endregion
    }
}
