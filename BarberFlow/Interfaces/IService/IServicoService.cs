using BarberFlow.API.DTOs.Servico;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IServicoService
    {
        #region Comandos (Escrita)

        Task<ServicoResponseDto> CriarServico(ServicoCreateDto dto);
        Task AtualizarServico(long id, ServicoUpdateDto dto);
        Task DeletarServico(long id);

        #endregion

        #region Consultas (Leitura)

        Task<IEnumerable<ServicoResponseDto>> ObterServicosPorEmpresaAdmin(long empresaId);
        Task<IEnumerable<ServicoResponseDto>> ObterServicosPorEmpresaCliente(long empresaId);

        #endregion
    }
}