using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.Enums;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IAgendamentoRepository
    {
        #region Persistência e Comandos
        Task Adicionar(Agendamento agendamento);
        Task Atualizar(Agendamento agendamento);
        Task<Agendamento?> ObterPorId(long id);
        #endregion

        #region Visão: Cliente
        Task<AgendamentoResponseDto?> ObterProximoAgendamentoCliente(long clienteId);
        Task<List<AgendamentoResponseDto>> ObterUltimosAgendamentosPorCliente(long clienteId, int quantidade);
        #endregion

        #region Visão: Profissional e Admin
        Task<List<AgendamentoResponseDto>> ObterAgendaPorPeriodo(long? profissionalId, long empresaId, DateOnly inicio, DateOnly fim, List<StatusAgendamento> statusFiltro, int pagina = 1);
        Task<decimal> ObterFaturamentoPorDia(long empresaId, DateOnly data);
        Task<int> ContarAgendamentoPorDia(long empresaId, DateOnly data);
        #endregion

        #region Validações e Regras de Conflito
        Task<bool> EstaOcupado(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null, long? bloqueioIdParaIgnorar = null);
        Task<bool> TemConflitoAgendamento(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null);
        Task<bool> TemConflitoBloqueio(long profissionalId, DateTime inicio, DateTime fim, long? bloqueioIdParaIgnorar = null);
        Task<bool> EstaForaDoHorarioTrabalho(long profissionalId, DateTime inicio, DateTime fim);
        #endregion
    }
}