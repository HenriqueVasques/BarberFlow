using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class HorarioProfissionalRepository : IHorarioProfissionalRepository
    {
        private readonly AppDbContext _appDbContext;

        public HorarioProfissionalRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Comandos (Escrita)

        // Adiciona uma nova configuração de horário para o profissional
        public async Task Adicionar(HorarioProfissional horarioPofissional)
        {
            await _appDbContext.HorarioProfissionais.AddAsync(horarioPofissional);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza os dados de horário (rastreado pelo EF)
        public async Task Atualizar(HorarioProfissional horarioPofissional)
        {
            _appDbContext.HorarioProfissionais.Update(horarioPofissional);
            await _appDbContext.SaveChangesAsync();
        }

        // Realiza o update para fins de Soft Delete ou inativação
        public async Task Deletar(HorarioProfissional horarioPofissional)
        {
            _appDbContext.HorarioProfissionais.Update(horarioPofissional);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas (Leitura e Validação)

        // Busca a Model com relacionamentos para validações complexas no Service
        public async Task<HorarioProfissional?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.HorarioProfissionais
                .Where(hp => hp.Id == id &&
                             (incluirDeletados || !hp.IsDeleted) &&
                             (!apenasAtivos || hp.Ativo)
                )
                .Include(hp => hp.Profissional)
                    .ThenInclude(p => p.HorariosProfissionais)
                .FirstOrDefaultAsync();
        }

        // Retorna listagem otimizada via DTO para exibição no sistema
        public async Task<List<HorarioProfissionalResponseDto>> ObterPorProfissionalId(long profissionalId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.HorarioProfissionais
                .AsNoTracking()
                .Where(hp => hp.ProfissionalId == profissionalId &&
                             (incluirDeletados || !hp.IsDeleted) &&
                             (!apenasAtivos || hp.Ativo)
                )
                .OrderBy(hp => hp.DiaSemana)
                .Select(hp => new HorarioProfissionalResponseDto
                {
                    Id = hp.Id,
                    ProfissionalId = hp.ProfissionalId,
                    NomeProfissional = hp.Profissional.Usuario.Nome,
                    DiaSemana = hp.DiaSemana,
                    HoraInicio = hp.HoraInicio,
                    HoraFim = hp.HoraFim,
                    HoraInicioAlmoco = hp.HoraInicioAlmoco,
                    HoraFimAlmoco = hp.HoraFimAlmoco,
                    Ativo = hp.Ativo
                })
                .ToListAsync();
        }

        #endregion
    }
}