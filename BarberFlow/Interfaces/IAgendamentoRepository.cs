using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IAgendamentoRepository
    {
        Task Adicionar(Agendamento agendamento);
        Task Atualizar(Agendamento agendamento);
        Task<Agendamento?> ObterPorId(long id);
        Task<IEnumerable<Agendamento>> ObterPorEmpresaId(long empresaId);
        Task<IEnumerable<Agendamento>> ObterPorProfissionalId(long profissionalId);
        Task<bool> EstaOcupado(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null, long? bloqueioIdParaIgnorar = null);
        Task<List<Agendamento>> ObterAgendaProfissionalPorData(long profissionalId, long empresaId, DateTime data);
        Task<bool> TemConflitoAgendamento(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null);
        Task<bool> TemConflitoBloqueio(long profissionalId, DateTime inicio, DateTime fim, long? bloqueioIdParaIgnorar = null);

    }
}
