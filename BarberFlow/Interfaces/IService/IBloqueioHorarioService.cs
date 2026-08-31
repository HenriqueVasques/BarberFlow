using BarberFlow.API.DTOs.BloqueioHorario;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IBloqueioHorarioService
    {
        #region Ações de Escrita (Regras de Negócio)

        Task<BloqueioHorarioResponseDto> CriarBloqueioHorario(BloqueioHorarioCreateDto dto);
        Task AtualizarBloqueioHorario(long id, BloqueioHorarioUpdateDto dto);
        Task DeletarBloqueioHorario(long id);

        #endregion

        #region Visão: Profissional / Admin (Agenda e Relatórios)

        Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorEmpresaId(long empresaId, DateOnly inicio, DateOnly fim, int pagina = 1);
        Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorProfissionalId(long profissionalId, DateOnly inicio, DateOnly fim, int pagina = 1);

        #endregion
    }
}