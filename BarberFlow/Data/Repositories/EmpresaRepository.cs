    using BarberFlow.API.Interfaces;
    using BarberFlow.API.Models;
    using Microsoft.EntityFrameworkCore;

    namespace BarberFlow.API.Data.Repositories
    {
        public class EmpresaRepository : IEmpresaRepository
        {
            private readonly AppDbContext _appDbContext;

            public EmpresaRepository(AppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }

            public async Task Adicionar(Empresa empresa)
            {
                await _appDbContext.Empresas.AddAsync(empresa);
                await _appDbContext.SaveChangesAsync();
            }

            public async Task<bool> ExisteSlug(string slug)
            {
                return await _appDbContext.Empresas.AnyAsync(e => e.Slug == slug);
            }

            public async Task<Empresa?> ObterPorId(long id)
            {
                return await _appDbContext.Empresas.FindAsync(id);
            }
        }
    }
