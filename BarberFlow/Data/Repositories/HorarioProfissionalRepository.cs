using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BarberFlow.API.Data.Repositories
{
    public class HorarioProfissionalRepository : IHorarioProfissionalRepository
    {
        private readonly AppDbContext _appDbContext;

        public HorarioProfissionalRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Adicionar(HorarioProfissional horarioPofissional)
        {
            await _appDbContext.HorarioProfissionais.AddAsync(horarioPofissional);
            await _appDbContext.SaveChangesAsync();

        }

        public async Task Atualizar(HorarioProfissional horarioPofissional)
        {
            _appDbContext.HorarioProfissionais.Update(horarioPofissional);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Deletar(HorarioProfissional horarioPofissional)
        {
            _appDbContext.HorarioProfissionais.Update(horarioPofissional);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<HorarioProfissional?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            var query = GerarQueryBase(apenasAtivos, incluirDeletados);
            return await query.FirstOrDefaultAsync(hp => hp.Id == id);
        }

        public async Task<List<HorarioProfissional>> ObterPorProfissionalId(long profissionalId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            var query = GerarQueryBase(apenasAtivos, incluirDeletados);
            return await query
                .Where(hp => hp.ProfissionalId == profissionalId)
                .ToListAsync();
        }

        #region Método Privado de Apoio
        private IQueryable<HorarioProfissional> GerarQueryBase(bool apenasAtivos, bool incluirDeletados)
        {
            IQueryable<HorarioProfissional> query = _appDbContext.HorarioProfissionais
                 .Include(hp => hp.Profissional).ThenInclude(hp => hp.HorariosProfissionais)
                 .AsNoTracking();

            if (!incluirDeletados)
                query = query.Where(hp => !hp.IsDeleted);

            if (apenasAtivos)
                query = query.Where(hp => hp.Ativo);

            return query;
        }
        #endregion
    }
}
