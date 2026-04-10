using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Repositories
{
    public class HorarioFuncionamentoEmpresaRepository : IHorarioFuncionamentoEmpresaRepository
    {
        private readonly AppDbContext _appDbContext;

        public HorarioFuncionamentoEmpresaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Comandos

        // Adiciona um novo horário de funcionamento ao banco
        public async Task Adicionar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa)
        {
            await _appDbContext.HorarioFuncionamentoEmpresas.AddAsync(horarioFuncionamentoEmpresa);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza os dados de um horário existente
        public async Task Atualizar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa)
        {
            _appDbContext.HorarioFuncionamentoEmpresas.Update(horarioFuncionamentoEmpresa);
            await _appDbContext.SaveChangesAsync();
        }

        // Realiza o soft delete ou atualização de status do horário
        public async Task Deletar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa)
        {
            _appDbContext.HorarioFuncionamentoEmpresas.Update(horarioFuncionamentoEmpresa);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas com Filtros Dinâmicos

        // Busca a entidade completa por ID para manipulação (rastreada pelo EF)
        public async Task<HorarioFuncionamentoEmpresa?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.HorarioFuncionamentoEmpresas
                .Where(hfe => hfe.Id == id &&
                             (incluirDeletados || !hfe.IsDeleted) &&
                             (!apenasAtivos || hfe.Ativo)
                )
                .FirstOrDefaultAsync();
        }

        // Busca o horário de um dia específico projetando diretamente para o DTO
        public async Task<HorarioFuncionamentoEmpresaResponseDto?> ObterPorDia(long empresaId, DayOfWeek diaDaSemana, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.HorarioFuncionamentoEmpresas
                .AsNoTracking()
                .Where(hfe => hfe.EmpresaId == empresaId &&
                               hfe.DiaSemana == diaDaSemana &&
                              (incluirDeletados || !hfe.IsDeleted) &&
                              (!apenasAtivos || hfe.Ativo)
                )
                .Select(hfe => new HorarioFuncionamentoEmpresaResponseDto
                {
                    Id = hfe.Id,
                    EmpresaId = hfe.EmpresaId,
                    DiaSemana = hfe.DiaSemana,
                    HoraAbertura = hfe.HoraAbertura,
                    HoraFechamento = hfe.HoraFechamento,
                    Ativo = hfe.Ativo,
                    EstaFechado = hfe.EstaFechado
                })
                .FirstOrDefaultAsync();
        }

        // Lista todos os horários da empresa projetando para DTO de forma performática
        public async Task<List<HorarioFuncionamentoEmpresaResponseDto>> ObterTodosPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.HorarioFuncionamentoEmpresas
                .AsNoTracking()
                .Where(hfe => hfe.EmpresaId == empresaId &&
                             (incluirDeletados || !hfe.IsDeleted) &&
                             (!apenasAtivos || hfe.Ativo)
                )
                .Select(hfe => new HorarioFuncionamentoEmpresaResponseDto
                {
                    Id = hfe.Id,
                    EmpresaId = hfe.EmpresaId,
                    DiaSemana = hfe.DiaSemana,
                    HoraAbertura = hfe.HoraAbertura,
                    HoraFechamento = hfe.HoraFechamento,
                    Ativo = hfe.Ativo,
                    EstaFechado = hfe.EstaFechado
                })
                .ToListAsync();
        }

        #endregion
    }
}