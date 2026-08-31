using BarberFlow.API.DTOs.Servico;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces.IRepository
{
    public interface IServicoRepository
    {
        #region Persistência e Comandos (Escrita)

        Task Adicionar(Servico servico);
        Task Atualizar(Servico servico);
        Task Deletar(Servico servico);

        #endregion

        #region Consultas (Leitura)

        Task<Servico?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<IEnumerable<ServicoResponseDto>> ObterPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false);

        #endregion
    }
}