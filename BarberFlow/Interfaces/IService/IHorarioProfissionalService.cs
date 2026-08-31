using BarberFlow.API.DTOs.HorarioProfissional;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IHorarioProfissionalService
    {
        #region Ações de Escrita (Admin)

        Task<HorarioProfissionalResponseDto> CriarHorarioProfissional(HorarioProfissionalCreateDto dto);
        Task AtualizarHorarioProfissional(long id, HorarioProfissionalUpdateDto dto);
        Task DeletarHorarioProfissional(long id);

        #endregion

        #region Consultas (Leitura)

        Task<HorarioProfissionalResponseDto> ObterPorId(long id);
        Task<List<HorarioProfissionalResponseDto>> ObterPorProfissionalId(long profissionalId);

        #endregion
    }
}