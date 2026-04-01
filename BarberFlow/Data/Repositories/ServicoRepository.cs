using BarberFlow.API.Data.Context;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class ServicoRepository : IServicoRepository
    {
        private readonly AppDbContext _appDbContext;

        public ServicoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Adicionar(Servico servico)
        {
            await _appDbContext.Servicos.AddAsync(servico);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Atualizar(Servico servico)
        {
            _appDbContext.Servicos.Update(servico);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Deletar(Servico servico)
        {
            _appDbContext.Servicos.Update(servico);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Servico>> ObterPorEmpresa(long empresaId)
        {
            return await _appDbContext.Servicos
            .Where(s => s.EmpresaId == empresaId && !s.IsDeleted && s.Ativo)
            .Include(s => s.Empresa)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<IEnumerable<Servico>> ObterPorEmpresaAdmin(long empresaId)
        {
            return await _appDbContext.Servicos
            .Where(s => s.EmpresaId == empresaId)
            .AsNoTracking()
            .ToListAsync();
        }

    public async Task<Servico?> ObterPorId(long id)
        {
            return await _appDbContext.Servicos
            .Include(s => s.Empresa)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted && s.Ativo);
        }
    }
}
