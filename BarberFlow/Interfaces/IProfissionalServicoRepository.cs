using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IProfissionalServicoRepository
    {
        Task Adicionar(ProfissionalServico profissionalServico);
        Task Atualizar(ProfissionalServico profissionalServico);
        Task Deletar(ProfissionalServico profissionalServico);

        Task<ProfissionalServico?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task <List<ProfissionalServico>> ObterPorProfissionalId(long profissionalId, bool apenasAtivos = true, bool incluirDeletados = false);
    }
}
