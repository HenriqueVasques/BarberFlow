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

            public async Task Atualizar(Empresa empresa)
            {
                _appDbContext.Empresas.Update(empresa);
                await _appDbContext.SaveChangesAsync();
            }

            public async Task Deletar(Empresa empresa)
            {
                _appDbContext.Empresas.Update(empresa);
                await _appDbContext.SaveChangesAsync();
            }

            public async Task<bool> ExisteSlug(string slug)
                {
                    return await _appDbContext.Empresas.AnyAsync(e => e.Slug == slug);
                }

            public async Task<Empresa?> ObterPorId(long id)
            {
                return await _appDbContext.Empresas
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
            }

            public async Task<bool> ExisteCnpj(string cnpj) => await _appDbContext.Empresas.AnyAsync(e => e.CNPJ == cnpj);

            public async Task<Empresa?> ObterPorSlug(string slug)
            {
                return await _appDbContext.Empresas.AsNoTracking().FirstOrDefaultAsync(e => e.Slug == slug.ToLower() && !e.IsDeleted && e.Ativo); 
            }

        }
    }
