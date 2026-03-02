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
            public async Task<IEnumerable<Servico>> ObterPorEmpresa(long empresaId)
            {
                return await _appDbContext.Servicos
                .Where(s => s.EmpresaId == empresaId)
                .AsNoTracking()
                .ToListAsync();
            }
        }
    }
