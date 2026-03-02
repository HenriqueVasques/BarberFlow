using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IServicoRepository
    {
        Task Adicionar(Servico servico);
        Task<IEnumerable<Servico>> ObterPorEmpresa(long empresaId);
    }
}
