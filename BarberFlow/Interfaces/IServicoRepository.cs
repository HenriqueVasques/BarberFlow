using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IServicoRepository
    {
        Task Adicionar(Servico servico);
        Task Atualizar(Servico servico);
        Task Deletar(Servico servico);
        Task<Servico?> ObterPorId(long id);
        Task<IEnumerable<Servico>> ObterPorEmpresa(long empresaId);
    }
}
