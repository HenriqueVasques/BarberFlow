using BarberFlow.API.Data.Context;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

public class HorarioFuncionamentoEmpresaRepository : IHorarioFuncionamentoEmpresaRepository
{
    private readonly AppDbContext _appDbContext;

    public HorarioFuncionamentoEmpresaRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    #region Comandos
    public async Task Adicionar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa)
    {
        await _appDbContext.HorarioFuncionamentoEmpresas.AddAsync(horarioFuncionamentoEmpresa);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Atualizar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa)
    {
        _appDbContext.HorarioFuncionamentoEmpresas.Update(horarioFuncionamentoEmpresa);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Deletar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa)
    {
        _appDbContext.HorarioFuncionamentoEmpresas.Update(horarioFuncionamentoEmpresa);
        await _appDbContext.SaveChangesAsync();
    }
    #endregion

    #region Consultas com Filtros Dinâmicos

    public async Task<HorarioFuncionamentoEmpresa?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
    {
        var query = GerarQueryBase(apenasAtivos, incluirDeletados);

        return await query.FirstOrDefaultAsync(hre => hre.Id == id);
    }

    public async Task<HorarioFuncionamentoEmpresa?> ObterPorDia(long empresaId, DayOfWeek diaDaSemana, bool apenasAtivos = true, bool incluirDeletados = false)
    {
        var query = GerarQueryBase(apenasAtivos, incluirDeletados);

        return await query.FirstOrDefaultAsync(hre => hre.EmpresaId == empresaId && hre.DiaSemana == diaDaSemana);
    }

    public async Task<List<HorarioFuncionamentoEmpresa>> ObterTodosPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false)
    {
        var query = GerarQueryBase(apenasAtivos, incluirDeletados);

        return await query
            .Where(hre => hre.EmpresaId == empresaId)
            .OrderBy(hre => hre.DiaSemana)
            .ToListAsync();
    }

    #endregion

    #region Método Privado de Apoio (O Segredo da Refatoração)

    // Este método centraliza a lógica de filtros para não repetir código em cada busca
    private IQueryable<HorarioFuncionamentoEmpresa> GerarQueryBase(bool apenasAtivos, bool incluirDeletados)
    {
        IQueryable<HorarioFuncionamentoEmpresa> query = _appDbContext.HorarioFuncionamentoEmpresas
            .Include(hre => hre.Empresa)
            .AsNoTracking();

        if (!incluirDeletados)
            query = query.Where(hre => !hre.IsDeleted);

        if (apenasAtivos)
            query = query.Where(hre => hre.Ativo);

        return query;
    }

    #endregion
}