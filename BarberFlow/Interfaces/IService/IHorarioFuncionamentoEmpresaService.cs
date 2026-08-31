using BarberFlow.API.DTOs;
using BarberFlow.API.DTOs.HorarioFuncionamentoEmpresa;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IHorarioFuncionamentoEmpresaService
    {
        #region Ações de Escrita (Admin)

        Task<HorarioFuncionamentoEmpresaResponseDto> CriarHorarioFuncionamentoEmpresa(HorarioFuncionamentoEmpresaCreateDto dto, long empresaId);
        Task AtualizarHorarioFuncionamentoEmpresa(HorarioFuncionamentoEmpresaUpdateDto dto, long id);
        Task DeletarHorarioFuncionamentoEmpresa(long id);

        #endregion

        #region Consultas e Visões

        Task<HorarioFuncionamentoEmpresaResponseDto?> ObterPorDia(long empresaId, DayOfWeek diaDaSemana);
        Task<List<HorarioFuncionamentoEmpresaResponseDto>> ObterPorEmpresa(long empresaId);
        Task<HorarioFuncionamentoEmpresaResponseDto> ObterPorId(long id);

        #endregion
    }
}