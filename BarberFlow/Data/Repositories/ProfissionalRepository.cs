using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Profissional;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class ProfissionalRepository : IProfissionalRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProfissionalRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Comandos (Escrita)

        // Adiciona um novo profissional no banco de dados
        public async Task Adicionar(Profissional profissional)
        {
            await _appDbContext.Profissionais.AddAsync(profissional);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza as propriedades de um profissional existente
        public async Task Atualizar(Profissional profissional)
        {
            _appDbContext.Profissionais.Update(profissional);
            await _appDbContext.SaveChangesAsync();
        }

        // Executa o update para aplicação do Soft Delete
        public async Task Deletar(Profissional profissional)
        {
            _appDbContext.Profissionais.Update(profissional);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas (Leitura)

        // Busca o Profissional por ID incluindo agregados necessários para regras de negócio no Service
        public async Task<Profissional?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.Profissionais
                .Where(p => p.Id == id &&
                            (incluirDeletados || !p.IsDeleted) &&
                            (!apenasAtivos || p.Ativo))
                .Include(p => p.Usuario)
                .Include(p => p.Empresa)
                .Include(p => p.HorariosProfissionais)
                .Include(p => p.ProfissionalServicos)
                .FirstOrDefaultAsync();
        }

        // Lista otimizada (AsNoTracking) de profissionais de uma determinada empresa projetada em DTO
        public async Task<IEnumerable<ProfissionalResponseDto>> ObterPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.Profissionais
                .AsNoTracking()
                .Where(p => p.EmpresaId == empresaId &&
                            (incluirDeletados || !p.IsDeleted) &&
                            (!apenasAtivos || p.Ativo))
                .Select(p => new ProfissionalResponseDto
                {
                    Id = p.Id,
                    Nome = p.Usuario.Nome,
                    NomeEmpresa = p.Empresa.Nome,
                    Email = p.Usuario.Email,
                    EmpresaId = p.EmpresaId,
                    UsuarioId = p.UsuarioId,
                    PercentualComissao = p.PercentualComissao,
                    DataCriacao = p.DataCriacao,
                    DataAtualizacao = p.DataAtualizacao,
                    Ativo = p.Ativo
                })
                .ToListAsync();
        }

        #endregion
    }
}