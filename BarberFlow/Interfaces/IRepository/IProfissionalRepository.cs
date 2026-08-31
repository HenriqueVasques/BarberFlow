using BarberFlow.API.DTOs.Profissional;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces.IRepository
{
    public interface IProfissionalRepository
    {
        #region Persistência e Comandos (Escrita)

        Task Adicionar(Profissional profissional);
        Task Atualizar(Profissional profissional);
        Task Deletar(Profissional profissional);

        #endregion

        #region Consultas (Leitura)

        Task<Profissional?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<IEnumerable<ProfissionalResponseDto>> ObterPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false);

        #endregion
    }
}