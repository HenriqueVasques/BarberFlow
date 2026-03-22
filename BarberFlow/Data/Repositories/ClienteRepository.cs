using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using BarberFlow.API.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _appDbContext;

        public ClienteRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Adicionar(Cliente cliente)
        {
            await _appDbContext.Clientes.AddAsync(cliente);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Atualizar(Cliente cliente)
        {
            _appDbContext.Clientes.Update(cliente);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Deletar(Cliente cliente)
        {
            _appDbContext.Clientes.Update(cliente);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cliente>> ObterPorEmpresa(long empresaId)
        {
            return await _appDbContext.Clientes
                .Where(c => c.EmpresaId == empresaId && !c.IsDeleted && c.Ativo)
                .Include(c => c.Empresa)
                .Include(c => c.Usuario)    
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Cliente?> ObterPorId(long id)
        {
            return await _appDbContext.Clientes
                .Where(c => c.Id == id && !c.IsDeleted && c.Ativo)
                .Include(c => c.Empresa)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync();
        }
    }
}
