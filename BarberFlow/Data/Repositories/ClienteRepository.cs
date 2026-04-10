using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Cliente;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        #region Readonly Fields
        private readonly AppDbContext _appDbContext;
        #endregion

        #region Construtor
        public ClienteRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Comandos (Escrita)

        // Insere um novo cliente no banco de dados
        public async Task Adicionar(Cliente cliente)
        {
            await _appDbContext.Clientes.AddAsync(cliente);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza os dados cadastrais do cliente
        public async Task Atualizar(Cliente cliente)
        {
            _appDbContext.Clientes.Update(cliente);
            await _appDbContext.SaveChangesAsync();
        }

        // Realiza o Soft Delete persistindo o estado alterado pelo Service
        public async Task Deletar(Cliente cliente)
        {
            _appDbContext.Clientes.Update(cliente);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas (Leitura)

        // Busca básica de cliente por ID com filtro opcional de deletados
        public async Task<Cliente?> ObterPorId(long id, bool incluirDeletados = false)
        {
            return await _appDbContext.Clientes
                .Where(c => c.Id == id && (incluirDeletados || !c.IsDeleted))
                .FirstOrDefaultAsync();
        }

        // Busca cliente incluindo os dados de Usuário para fluxos de edição/LGPD
        public async Task<Cliente?> ObterClienteComUsuario(long id)
        {
            return await _appDbContext.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        // Lista clientes de uma empresa com paginação e projeção para DTO
        public async Task<IEnumerable<ClienteResponseDto>> ObterPorEmpresa(long empresaId, bool incluirDeletados = false, int pagina = 1)
        {
            return await _appDbContext.Clientes
                .AsNoTracking()
                .Where(c => c.EmpresaId == empresaId && (incluirDeletados || !c.IsDeleted))
                .OrderBy(c => c.Usuario.Nome)
                .Skip((pagina - 1) * 10)
                .Take(10)
                .Select(c => new ClienteResponseDto
                {
                    Id = c.Id,
                    Nome = c.Usuario.Nome,
                    Email = c.Usuario.Email,
                    Telefone = c.Usuario.Telefone,
                    Whatsapp = c.Usuario.Whatsapp,
                    EmpresaId = c.EmpresaId,
                    UsuarioId = c.UsuarioId,
                    DataCriacao = c.DataCriacao,
                    DataAtualizacao = c.DataAtualizacao,
                    Ativo = c.Ativo
                })
                .ToListAsync();
        }

        #endregion
    }
}