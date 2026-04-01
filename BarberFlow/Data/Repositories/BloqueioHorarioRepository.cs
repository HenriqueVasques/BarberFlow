using BarberFlow.API.Data.Context;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class BloqueioHorarioRepository : IBloqueioHorarioRepository
    {
        private readonly AppDbContext _appDbContext;

       public BloqueioHorarioRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Adicionar(BloqueioHorario bloqueio)
        {
           await _appDbContext.BloqueioHorarios.AddAsync(bloqueio);
           await _appDbContext.SaveChangesAsync();
        }

        public async Task Atualizar(BloqueioHorario bloqueio)
        {
            _appDbContext.BloqueioHorarios.Update(bloqueio);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Deletar(BloqueioHorario bloqueio)
        {
            _appDbContext.BloqueioHorarios.Update(bloqueio);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BloqueioHorario>> ObterPorEmpresaId(long empresaId)
        {
            return await _appDbContext.BloqueioHorarios
                .Where(b=> b.EmpresaId == empresaId && !b.IsDeleted)
                .Include(b => b.Empresa)
                .Include(b => b.Profissional).ThenInclude(b => b.Usuario)
                .OrderBy(b => b.DataHoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BloqueioHorario?> ObterPorId(long id)
        {
            return await _appDbContext.BloqueioHorarios
                .Include(b => b.Profissional)
                .ThenInclude(p => p.Usuario)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<IEnumerable<BloqueioHorario>> ObterPorProfissionalId(long profissionalId)
        {
            return await _appDbContext.BloqueioHorarios
                .Where(b => b.ProfissionalId == profissionalId && !b.IsDeleted)
                .Include(b => b.Empresa)
                .Include(b => b.Profissional).ThenInclude(b => b.Usuario)
                .OrderBy(b => b.DataHoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
