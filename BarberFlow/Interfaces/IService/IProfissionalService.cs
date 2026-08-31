using BarberFlow.API.DTOs.Profissional;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IProfissionalService
    {
        #region Comandos: Escrita (Admin / Gestão)

        Task<ProfissionalResponseDto> CriarProfissional(ProfissionalCreateDto dto);
        Task AtualizarProfissional(long id, ProfissionalUpdateDto dto);
        Task DeletarProfissional(long id);

        #endregion

        #region Consultas: Leitura

        Task<ProfissionalResponseDto> ObterPorId(long id);
        Task<IEnumerable<ProfissionalResponseDto>> ObterProfissionaisPorEmpresa(long empresaId);

        #endregion
    }
}