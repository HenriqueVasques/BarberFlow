using BarberFlow.API.DTOs.Cliente;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces.IRepository
{
    public interface IClienteRepository
    {
        #region Operações de Escrita

        Task Adicionar(Cliente cliente);
        Task Atualizar(Cliente cliente);
        Task Deletar(Cliente cliente);

        #endregion

        #region Operações de Leitura

        Task<Cliente?> ObterPorId(long id, bool incluirDeletados = false);
        Task<Cliente?> ObterClienteComUsuario(long id);
        Task<IEnumerable<ClienteResponseDto>> ObterPorEmpresa(long empresaId, int pagina = 1, bool incluirDeletados = false);

        #endregion
    }
}