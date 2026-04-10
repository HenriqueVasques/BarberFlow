using BarberFlow.API.DTOs;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IHorarioFuncionamentoEmpresaRepository
    {
        #region Persistência e Comandos
        Task Adicionar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa);
        Task Atualizar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa);
        Task Deletar(HorarioFuncionamentoEmpresa horarioFuncionamentoEmpresa);
        #endregion

        #region Consultas Dinâmicas
        Task<HorarioFuncionamentoEmpresa?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<List<HorarioFuncionamentoEmpresaResponseDto>> ObterTodosPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<HorarioFuncionamentoEmpresaResponseDto?> ObterPorDia(long empresaId, DayOfWeek diaDaSemana, bool apenasAtivos = true, bool incluirDeletados = false);
        #endregion
    }
}
