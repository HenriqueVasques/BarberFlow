using BarberFlow.API.DTOs.BloqueioHorario;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces.IRepository
{
    public interface IBloqueioHorarioRepository
    {
        #region Ações de Escrita (Persistência)
        Task Adicionar(BloqueioHorario bloqueio);
        Task Atualizar(BloqueioHorario bloqueio);
        Task Deletar(BloqueioHorario bloqueio);
        #endregion

        #region Visão: Geral (Consultas de Entidade)
        Task<BloqueioHorario?> ObterPorId(long id, bool incluirDeletados = false);
        #endregion

        #region Visão: Admin / Profissional (Listagens e Filtros)
        Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorEmpresaId(long empresaId, DateOnly inicio, DateOnly fim, int pagina = 1, bool incluirDeletados = false);
        Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorProfissionalId(long profissionalId, DateOnly inicio, DateOnly fim, int pagina = 1, bool incluirDeletados = false);
        #endregion
    }
}