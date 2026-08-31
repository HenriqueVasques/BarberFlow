using BarberFlow.API.DTOs.Empresa;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IEmpresaService
    {
        #region Operações de Escrita (Ações de Comando)

        Task<EmpresaResponseDto> CriarEmpresa(EmpresaCreateDto dto);
        Task AtualizarEmpresa(long id, EmpresaUpdateDto dto);
        Task Deletar(long id);

        #endregion

        #region Operações de Leitura (Consultas)

        Task<EmpresaResponseDto?> ObterEmpresaPorSlug(string slug);
        Task<EmpresaResponseDto?> ObterPorId(long id);

        #endregion
    }
}