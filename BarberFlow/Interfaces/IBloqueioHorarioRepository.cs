using BarberFlow.API.Controllers;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
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
        Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorEmpresaId(long empresaId, DateOnly inicio, DateOnly fim, bool incluirDeletados = false, int pagina = 1);
        Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorProfissionalId(long profissionalId, DateOnly inicio, DateOnly fim, bool incluirDeletados = false, int pagina = 1);
        #endregion
    }
}