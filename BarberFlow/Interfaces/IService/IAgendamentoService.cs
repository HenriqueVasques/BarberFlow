using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.Enums;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IAgendamentoService
    {
        #region Ações de Escrita (Regras de Negócio)

        Task<AgendamentoResponseDto> CriarAgendamento(AgendamentoCreateDto dto);
        Task Cancelar(long id);
        Task Finalizar(long id);

        #endregion

        #region Visão: Geral

        Task<AgendamentoResponseDto> ObterPorId(long id);

        #endregion

        #region Visão: Cliente

        Task<AgendamentoResponseDto?> ObterProximoAgendamentoCliente(long clienteId);
        Task<List<AgendamentoResponseDto>> ObterUltimosAgendamentosPorCliente(long clienteId);

        #endregion

        #region Visão: Profissional / Admin (Agenda e Relatórios)

        Task<List<AgendamentoResponseDto>> ObterAgendaPorPeriodo(long? profissionalId, long empresaId, DateOnly inicio, DateOnly fim, List<StatusAgendamento> statusFiltro);
        Task<DashboardResumoDto> ObterResumoPorDia(long empresaId, DateOnly data);

        #endregion
    }
}