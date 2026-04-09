using BarberFlow.API.DTOs.Empresa;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IEmpresaRepository
    {
        #region Operações de Escrita (Comandos)

        Task Adicionar(Empresa empresa);
        Task Atualizar(Empresa empresa);
        Task Deletar(Empresa empresa);

        #endregion

        #region Operações de Leitura (Consultas e Validações)

        Task<bool> ExisteSlug(string slug);
        Task<bool> ExisteCnpj(string cnpj);
        Task<Empresa?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<Empresa?> ObterPorIdComHorarioEmpresa(long id);
        Task<EmpresaResponseDto?> ObterPorSlug(string slug);

        #endregion
    }
}