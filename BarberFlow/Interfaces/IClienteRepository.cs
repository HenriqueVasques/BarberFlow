using BarberFlow.API.Data.Repositories;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IClienteRepository
    {
        Task Adicionar(Cliente cliente);
        Task Atualizar(Cliente cliente);
        Task Deletar(Cliente cliente);
        Task<Cliente?> ObterPorId(long id);
        Task<IEnumerable<Cliente>> ObterPorEmpresa(long empresaId);
    }
}
