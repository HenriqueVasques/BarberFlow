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
        Task<Agendamento?> ObterProximoAgendamentoCliente(long clienteId);
        Task<List<Agendamento>> ObterUltimosPorCliente(long clienteId, int quantidade);
        #endregion

        #region Visão: Profissional e Admin
        Task<List<Agendamento>> ObterAgendaPorPeriodo(long? profissionalId, long empresaId, DateTime inicio, DateTime fim, List<StatusAgendamento> statusFiltro);
        Task<decimal> ObterFaturamentoPorDia(long empresaId, DateTime data);
        Task<int> ContarAgendamentoPorDia(long empresaId, DateTime data);
        #endregion

        #region Validações e Regras de Conflito
        Task<bool> EstaOcupado(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null, long? bloqueioIdParaIgnorar = null);
        Task<bool> TemConflitoAgendamento(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null);
        Task<bool> TemConflitoBloqueio(long profissionalId, DateTime inicio, DateTime fim, long? bloqueioIdParaIgnorar = null);
        #endregion
    }
}