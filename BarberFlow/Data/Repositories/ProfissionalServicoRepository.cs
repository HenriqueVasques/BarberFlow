using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.ProfissionalServico;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class ProfissionalServicoRepository : IProfissionalServicoRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProfissionalServicoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Comandos (Escrita)

        // Adiciona um novo vínculo de serviço ao profissional no banco de dados
        public async Task Adicionar(ProfissionalServico profissionalServico)
        {
            await _appDbContext.ProfissionalServicos.AddAsync(profissionalServico);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza as configurações personalizadas (preço/duração/status) do vínculo
        public async Task Atualizar(ProfissionalServico profissionalServico)
        {
            _appDbContext.ProfissionalServicos.Update(profissionalServico);
            await _appDbContext.SaveChangesAsync();
        }

        // Executa o update para fins de Soft Delete ou inativação
        public async Task Deletar(ProfissionalServico profissionalServico)
        {
            _appDbContext.ProfissionalServicos.Update(profissionalServico);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas (Leitura)

        // Busca a entidade ProfissionalServico por ID incluindo navegações necessárias para validações
        public async Task<ProfissionalServico?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.ProfissionalServicos
                .Where(ps => ps.Id == id &&
                            (incluirDeletados || !ps.IsDeleted) &&
                            (!apenasAtivos || ps.Ativo))
                .Include(ps => ps.Servico)
                .Include(ps => ps.Profissional)
                    .ThenInclude(p => p.Usuario)
                .FirstOrDefaultAsync();
        }

        // Retorna a listagem otimizada (AsNoTracking) de serviços prestados por um profissional específico
        public async Task<List<ProfissionalServicoResponseDto>> ObterPorProfissionalId(long profissionalId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.ProfissionalServicos
                .AsNoTracking()
                .Where(ps => ps.ProfissionalId == profissionalId &&
                            (incluirDeletados || !ps.IsDeleted) &&
                            (!apenasAtivos || ps.Ativo))
                .Select(ps => new ProfissionalServicoResponseDto
                {
                    Id = ps.Id,
                    ProfissionalId = ps.ProfissionalId,
                    ServicoId = ps.ServicoId,
                    NomeServico = ps.Servico.Nome,
                    NomeProfissional = ps.Profissional.Usuario.Nome,
                    PrecoPersonalizado = ps.PrecoPersonalizado,
                    DuracaoPersonalizadaMinutos = ps.DuracaoPersonalizadaMinutos,
                    Ativo = ps.Ativo,
                    DataCriacao = ps.DataCriacao,
                    DataAtualizacao = ps.DataAtualizacao
                })
                .ToListAsync();
        }
        #endregion
    }
}