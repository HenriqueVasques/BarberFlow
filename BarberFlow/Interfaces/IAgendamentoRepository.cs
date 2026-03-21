using BarberFlow.API.Enums;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IAgendamentoRepository
    {
        Task Adicionar(Agendamento agendamento);
        Task Atualizar(Agendamento agendamento);
        Task<Agendamento?> ObterPorId(long id);
        Task<List<Agendamento>> ObterAgendaPorPeriodo(long profissionalId, long empresaId, DateTime inicio, DateTime fim, List<StatusAgendamento> statusFiltro);

        Task<bool> EstaOcupado(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null, long? bloqueioIdParaIgnorar = null);
        Task<bool> TemConflitoAgendamento(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null);
        Task<bool> TemConflitoBloqueio(long profissionalId, DateTime inicio, DateTime fim, long? bloqueioIdParaIgnorar = null);
    }
}