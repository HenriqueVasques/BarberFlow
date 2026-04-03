using BarberFlow.API.Controllers;
using BarberFlow.API.Data.Context;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class BloqueioHorarioRepository : IBloqueioHorarioRepository
    {
        #region Readonly Fields
        private readonly AppDbContext _appDbContext;
        #endregion

        #region Construtor
        public BloqueioHorarioRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Comandos (Escrita)

        // Insere um novo bloqueio de horário no banco
        public async Task Adicionar(BloqueioHorario bloqueio)
        {
            await _appDbContext.BloqueioHorarios.AddAsync(bloqueio);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza os dados de um bloqueio existente
        public async Task Atualizar(BloqueioHorario bloqueio)
        {
            _appDbContext.BloqueioHorarios.Update(bloqueio);
            await _appDbContext.SaveChangesAsync();
        }

        // Realiza o Soft Delete de um bloqueio
        public async Task Deletar(BloqueioHorario bloqueio)
        {
            _appDbContext.BloqueioHorarios.Update(bloqueio);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas: Visão Geral

        // Busca a entidade pura por ID para manipulação
        public async Task<BloqueioHorario?> ObterPorId(long id, bool incluirDeletados = false)
        {
            return await _appDbContext.BloqueioHorarios
                .Where(b => b.Id == id && (incluirDeletados || !b.IsDeleted))
                .FirstOrDefaultAsync();
        }

        // Lista bloqueios de uma empresa por período com projeção para DTO
        public async Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorEmpresaId(long empresaId, DateOnly inicio, DateOnly fim, bool incluirDeletados = false, int pagina = 1)
        {
            return await _appDbContext.BloqueioHorarios
                .AsNoTracking()
                .Where(b => b.EmpresaId == empresaId &&
                            b.DataHoraInicio <= fim.ToDateTime(TimeOnly.MaxValue) &&
                            b.DataHoraInicio >= inicio.ToDateTime(TimeOnly.MinValue) &&
                            (incluirDeletados || !b.IsDeleted))
                .OrderByDescending(b => b.DataHoraInicio)
                .Skip((pagina - 1) * 10)
                .Take(10)
                .Select(b => new BloqueioHorarioResponseDto
                {
                    Id = b.Id,
                    ProfissionalId = b.ProfissionalId,
                    EmpresaId = b.EmpresaId,
                    NomeProfissional = b.Profissional.Usuario.Nome,
                    NomeEmpresa = b.Empresa.Nome,
                    DataHoraInicio = b.DataHoraInicio,
                    DataHoraFim = b.DataHoraFim,
                    DataCriacao = b.DataCriacao,
                    DataAtualizacao = b.DataAtualizacao,
                    Motivo = b.Motivo
                })
                .ToListAsync();
        }

        // Lista bloqueios de um profissional por período com projeção para DTO
        public async Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorProfissionalId(long profissionalId, DateOnly inicio, DateOnly fim, bool incluirDeletados = false, int pagina = 1)
        {
            return await _appDbContext.BloqueioHorarios
                .AsNoTracking()
                .Where(b => b.ProfissionalId == profissionalId &&
                            b.DataHoraInicio >= inicio.ToDateTime(TimeOnly.MinValue) &&
                            b.DataHoraInicio <= fim.ToDateTime(TimeOnly.MaxValue) &&
                            (incluirDeletados || !b.IsDeleted))
                .OrderBy(b => b.DataHoraInicio)
                .Skip((pagina - 1) * 10)
                .Take(10)
                .Select(b => new BloqueioHorarioResponseDto
                {
                    Id = b.Id,
                    ProfissionalId = b.ProfissionalId,
                    EmpresaId = b.EmpresaId,
                    NomeProfissional = b.Profissional.Usuario.Nome,
                    NomeEmpresa = b.Empresa.Nome,
                    DataHoraInicio = b.DataHoraInicio,
                    DataHoraFim = b.DataHoraFim,
                    DataCriacao = b.DataCriacao,
                    DataAtualizacao = b.DataAtualizacao,
                    Motivo = b.Motivo
                })
                .ToListAsync();
        }

        #endregion
    }
}