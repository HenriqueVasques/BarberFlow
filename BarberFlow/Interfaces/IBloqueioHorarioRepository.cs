using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IBloqueioHorarioRepository
    {
            Task Adicionar(BloqueioHorario bloqueio);
            Task Atualizar(BloqueioHorario bloqueio);
            Task Deletar(BloqueioHorario bloqueio);
            Task<BloqueioHorario?> ObterPorId(long id);
            Task<IEnumerable<BloqueioHorario>> ObterPorEmpresaId(long empresaId);
            Task<IEnumerable<BloqueioHorario>> ObterPorProfissionalId(long profissionalId);
    }
}
