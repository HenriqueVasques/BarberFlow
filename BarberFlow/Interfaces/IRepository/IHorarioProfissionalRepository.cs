using BarberFlow.API.DTOs.HorarioProfissional;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces.IRepository
{
    public interface IHorarioProfissionalRepository
    {
        #region Persistência e Comandos (Escrita)

        Task Adicionar(HorarioProfissional horarioPofissional);
        Task Atualizar(HorarioProfissional horarioPofissional);
        Task Deletar(HorarioProfissional horarioPofissional);

        #endregion

        #region Consultas (Leitura)

        Task<HorarioProfissional?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<List<HorarioProfissionalResponseDto>> ObterPorProfissionalId(long profissionalId, bool apenasAtivos = true, bool incluirDeletados = false);

        #endregion
    }
}