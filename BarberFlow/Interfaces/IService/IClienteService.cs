using BarberFlow.API.DTOs.Cliente;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IClienteService
    {
        #region Ações de Escrita (Regras de Negócio)

        Task<ClienteResponseDto> CriarCliente(ClienteCreateDto dto);
        Task AtualizarCliente(long id, ClienteUpdateDto dto);
        Task DeletarCliente(long id);

        #endregion

        #region Consultas (Leitura)

        Task<ClienteResponseDto> ObterClientePorId(long id);
        Task<IEnumerable<ClienteResponseDto>> ObterClientesPorEmpresa(long empresaId, int pagina = 1);

        #endregion
    }
}