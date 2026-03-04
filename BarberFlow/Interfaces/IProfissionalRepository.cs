using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IProfissionalRepository
    {
        Task Adicionar(Profissional profissional);
        Task Atualizar(Profissional profissional);
        Task Deletar(Profissional profissional);
        Task<Profissional?> ObterPorId(long id);
        Task<IEnumerable<Profissional>> ObterPorEmpresa(long empresaId);
    }
}
