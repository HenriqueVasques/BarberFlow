using BarberFlow.API.Data.Context;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class ProfissionalRepository : IProfissionalRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProfissionalRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Adicionar(Profissional profissional)
        {
            await _appDbContext.Profissionais.AddAsync(profissional);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Atualizar(Profissional profissional)
        {
            _appDbContext.Profissionais.Update(profissional);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Deletar(Profissional profissional)
        {
            _appDbContext.Profissionais.Update(profissional);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Profissional>> ObterPorEmpresa(long empresaId)
        {
            return await _appDbContext.Profissionais
                .Where(p => p.EmpresaId == empresaId && !p.IsDeleted && p.Ativo)
                .Include(p => p.Usuario)
                .Include(p => p.Empresa)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Profissional?> ObterPorId(long id)
        {
            return await _appDbContext.Profissionais
                .Include(p => p.Usuario)
                .Include(p => p.Empresa)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted && p.Ativo);
        }
    }
}
