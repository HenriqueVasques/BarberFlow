using BarberFlow.API.Data.Repositories;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IHorarioProfissionalRepository
    {
        Task Adicionar(HorarioProfissional horarioPofissional);
        Task Atualizar(HorarioProfissional horarioPofissional);
        Task Deletar(HorarioProfissional horarioPofissional);

        Task<HorarioProfissional?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<List<HorarioProfissional>> ObterPorProfissionalId(long profissionalId, bool apenasAtivos = true, bool incluirDeletados = false);
    }
}
