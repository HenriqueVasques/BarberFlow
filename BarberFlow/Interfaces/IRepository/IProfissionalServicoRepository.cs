using BarberFlow.API.DTOs.ProfissionalServico;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces.IRepository
{
    public interface IProfissionalServicoRepository
    {
        #region Persistência e Comandos (Escrita)

        Task Adicionar(ProfissionalServico profissionalServico);
        Task Atualizar(ProfissionalServico profissionalServico);
        Task Deletar(ProfissionalServico profissionalServico);

        #endregion

        #region Consultas (Leitura)

        Task<ProfissionalServico?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<List<ProfissionalServicoResponseDto>> ObterPorProfissionalId(long profissionalId, bool apenasAtivos = true, bool incluirDeletados = false);

        #endregion
    }
}