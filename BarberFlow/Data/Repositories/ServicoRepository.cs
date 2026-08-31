using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Servico;
using BarberFlow.API.Interfaces.IRepository;
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

        #region Comandos (Escrita)

        // Adiciona um novo serviço no banco de dados
        public async Task Adicionar(Servico servico)
        {
            await _appDbContext.Servicos.AddAsync(servico);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza as propriedades de um serviço existente
        public async Task Atualizar(Servico servico)
        {
            _appDbContext.Servicos.Update(servico);
            await _appDbContext.SaveChangesAsync();
        }

        // Executa o update para aplicação do Soft Delete ou alteração de status
        public async Task Deletar(Servico servico)
        {
            _appDbContext.Servicos.Update(servico);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas (Leitura)

        // Busca a entidade Servico por ID incluindo dados da Empresa para validações no Service
        public async Task<Servico?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.Servicos
                .Where(s => s.Id == id &&
                            (incluirDeletados || !s.IsDeleted) &&
                            (!apenasAtivos || s.Ativo))
                .Include(s => s.Empresa)
                .FirstOrDefaultAsync();
        }

        // Retorna a listagem otimizada (AsNoTracking) dos serviços cadastrados para uma determinada empresa
        public async Task<IEnumerable<ServicoResponseDto>> ObterPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.Servicos
                .AsNoTracking()
                .Where(s => s.EmpresaId == empresaId &&
                            (incluirDeletados || !s.IsDeleted) &&
                            (!apenasAtivos || s.Ativo))
                .Select(s => new ServicoResponseDto
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    NomeEmpresa = s.Empresa.Nome,
                    DuracaoMinutos = s.DuracaoMinutos,
                    PrecoBase = s.PrecoBase,
                    DataCriacao = s.DataCriacao,
                    DataAtualizacao = s.DataAtualizacao,
                    Ativo = s.Ativo
                })
                .ToListAsync();
        }

        #endregion
    }
}