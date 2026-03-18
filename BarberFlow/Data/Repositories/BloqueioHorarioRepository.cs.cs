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
           await _appDbContext.Bloqueio_Horarios.AddAsync(bloqueio);
           await _appDbContext.SaveChangesAsync();
        }

        public async Task Atualizar(BloqueioHorario bloqueio)
        {
            _appDbContext.Bloqueio_Horarios.Update(bloqueio);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Deletar(BloqueioHorario bloqueio)
        {
            _appDbContext.Bloqueio_Horarios.Update(bloqueio);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BloqueioHorario>> ObterPorEmpresaId(long empresaId)
        {
            return await _appDbContext.Bloqueio_Horarios
                .Where(b=> b.EmpresaId == empresaId)
                .Include(b => b.Profissional)
                .OrderBy(b => b.DataHoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BloqueioHorario?> ObterPorId(long id)
        {
            return await _appDbContext.Bloqueio_Horarios
                .Include(b => b.Profissional)
                .ThenInclude(p => p.Usuario)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<IEnumerable<BloqueioHorario>> ObterPorProfissionalId(long profissionalId)
        {
            return await _appDbContext.Bloqueio_Horarios
                .Where(b => b.ProfissionalId == profissionalId)
                .Include(b => b.Profissional)
                .OrderBy(b => b.DataHoraInicio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
