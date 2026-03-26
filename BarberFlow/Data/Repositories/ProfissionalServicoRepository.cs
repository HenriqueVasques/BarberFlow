using BarberFlow.API.Data.Context;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class ProfissionalServicoRepository : IProfissionalServicoRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProfissionalServicoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Adicionar(ProfissionalServico profissionalServico)
        {
            await _appDbContext.ProfissionalServicos.AddAsync(profissionalServico);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Atualizar(ProfissionalServico profissionalServico)
        {
            _appDbContext.ProfissionalServicos.Update(profissionalServico);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Deletar(ProfissionalServico profissionalServico)
        {
            _appDbContext.ProfissionalServicos.Update(profissionalServico);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ProfissionalServico?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            var query = GerarQueryBase(apenasAtivos, incluirDeletados);
            return await query.FirstOrDefaultAsync(ps => ps.Id == id);
        }

        public async Task<List<ProfissionalServico>> ObterPorProfissionalId(long profissionalId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            var query = GerarQueryBase(apenasAtivos, incluirDeletados);
            return await query
                .Where(ps => ps.ProfissionalId == profissionalId)
                .ToListAsync();                   
        }

        #region Método Privado de Apoio

        // Este método centraliza a lógica de filtros para não repetir código em cada busca
        private IQueryable<ProfissionalServico> GerarQueryBase(bool apenasAtivos, bool incluirDeletados)
        {
            IQueryable<ProfissionalServico> query = _appDbContext.ProfissionalServicos
                 .Include(ps => ps.Servico)
                 .Include(ps => ps.Profissional).ThenInclude(ps => ps.Usuario)
                 .AsNoTracking();

            if (!incluirDeletados)
                query = query.Where(ps => !ps.IsDeleted);

            if (apenasAtivos)
                query = query.Where(ps => ps.Ativo);

            return query;
        }

        #endregion
    }
}
