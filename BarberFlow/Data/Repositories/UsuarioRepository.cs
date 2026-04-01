using BarberFlow.API.Data.Context;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _appDbContext;

        public UsuarioRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Adicionar(Usuario usuario)
        {
            await _appDbContext.Usuarios.AddAsync(usuario);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task AlterarSenha(Usuario usuario)
        {
             _appDbContext.Usuarios.Update(usuario);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Atualizar(Usuario usuario)
        {
            _appDbContext.Usuarios.Update(usuario);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Deletar(Usuario usuario)
        {
            _appDbContext.Usuarios.Update(usuario);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExisteEmail(string email)
        {
            if (await _appDbContext.Usuarios.AnyAsync(u => u.Email == email && !u.IsDeleted && u.Ativo))
            {
                return true;
            }
            return false;
        }

        public async Task<Usuario?> ObterPorId(long id)
        {
            return await _appDbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.Ativo);  
        }

        public async Task<IEnumerable<Usuario>> ObterPorEmpresa(long empresaId)
        {
            return await _appDbContext.Usuarios
                .Where(u => u.EmpresaId == empresaId && !u.IsDeleted && u.Ativo)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
