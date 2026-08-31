using BarberFlow.API.DTOs.ProfissionalServico;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IProfissionalServicoService
    {
        #region Comandos (Escrita)

        Task<ProfissionalServicoResponseDto> CriarProfissionalServico(ProfissionalServicoCreateDto dto);
        Task AtualizarProfissionalServico(long id, ProfissionalServicoUpdateDto dto);
        Task DeletarProfissionalServico(long id);

        #endregion

        #region Consultas (Leitura)

        Task<ProfissionalServicoResponseDto> ObterPorIdAdmin(long id);
        Task<ProfissionalServicoResponseDto> ObterPorIdCliente(long id);
        Task<List<ProfissionalServicoResponseDto>> ObterPorProfissionalIdAdmin(long profissionalId);
        Task<List<ProfissionalServicoResponseDto>> ObterPorProfissionalIdCliente(long profissionalId);

        #endregion
    }
}