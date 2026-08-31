using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Auth;
using BarberFlow.API.DTOs.Usuario;
using BarberFlow.API.Interfaces.IRepository;
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

        #region Comandos (Escrita)

        // Adiciona um novo usuário no banco de dados
        public async Task Adicionar(Usuario usuario)
        {
            await _appDbContext.Usuarios.AddAsync(usuario);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza os dados cadastrais do usuário
        public async Task Atualizar(Usuario usuario)
        {
            _appDbContext.Usuarios.Update(usuario);
            await _appDbContext.SaveChangesAsync();
        }

        // Executa o update para fins de Soft Delete ou inativação do usuário
        public async Task Deletar(Usuario usuario)
        {
            _appDbContext.Usuarios.Update(usuario);
            await _appDbContext.SaveChangesAsync();
        }

        // Persiste a alteração da senha do usuário
        public async Task AlterarSenha(Usuario usuario)
        {
            _appDbContext.Usuarios.Update(usuario);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas (Leitura)

        // Verifica se já existe um usuário ativo registrado com o e-mail informado
        public async Task<bool> ExisteEmail(string email)
        {
            return await _appDbContext.Usuarios
                .AnyAsync(u => u.Email == email && !u.IsDeleted && u.Ativo);
        }

        // Busca um usuário por ID aplicando filtros de status e soft delete
        public async Task<Usuario?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.Usuarios
                .Where(u => u.Id == id &&
                            (!apenasAtivos || u.Ativo) &&
                            (incluirDeletados || !u.IsDeleted))
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario?> ObterPorEmail(string email)
        {
            return await _appDbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower().Trim() && !u.IsDeleted);
        }

        // Retorna a listagem otimizada (AsNoTracking) de usuários vinculados a uma empresa
        public async Task<List<UsuarioResponseDto>> ObterPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.Usuarios
                .AsNoTracking()
                .Where(u => u.EmpresaId == empresaId &&
                            (!apenasAtivos || u.Ativo) &&
                            (incluirDeletados || !u.IsDeleted))
                .Select(u => new UsuarioResponseDto
                {
                    EmpresaId = u.EmpresaId,
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Perfil = u.Perfil,
                    Telefone = u.Telefone,
                    Whatsapp = u.Whatsapp,
                    DataCriacao = u.DataCriacao,
                    DataAtualizacao = u.DataAtualizacao,
                    IsDeleted = u.IsDeleted,
                    Ativo = u.Ativo
                })
                .ToListAsync();
        }

        #endregion
    }
}