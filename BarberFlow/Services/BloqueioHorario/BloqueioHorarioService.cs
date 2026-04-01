using BarberFlow.API.DTOs.BloqueioHorario;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class BloqueioHorarioService
    {
        #region Readonly Fields
        private readonly IBloqueioHorarioRepository _repositoryBloqueioHorario;
        private readonly IAgendamentoRepository _repositoryAgendamento;
        private readonly IProfissionalRepository _repositoryProfissional;
        private readonly IEmpresaRepository _repositoryEmpresa;
        #endregion

        #region Constructor
        public BloqueioHorarioService(IBloqueioHorarioRepository repositoryBloqueioHorario, IAgendamentoRepository repositoryAgendamento, IProfissionalRepository repositoryProfissional, IEmpresaRepository repositoryEmpresa)
        {
            _repositoryBloqueioHorario = repositoryBloqueioHorario;
            _repositoryAgendamento = repositoryAgendamento;
            _repositoryProfissional = repositoryProfissional;
            _repositoryEmpresa = repositoryEmpresa;
        }
        #endregion

        #region Public Methods
        public async Task<BloqueioHorario> CriarBloqueioHorario(BloqueioHorarioCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os ados não foram preenchidos.");

            var empresa = await _repositoryEmpresa.ObterPorId(dto.EmpresaId)
                ?? throw new Exception($"Empresa com id {dto.EmpresaId} não encontrada.");

            var profissional = await _repositoryProfissional.ObterPorId(dto.ProfissionalId)
                ?? throw new Exception($"Profissional com id {dto.ProfissionalId} não encontrado.");

            var ocupado = await _repositoryAgendamento.EstaOcupado(profissional.Id, dto.DataHoraInicio, dto.DataHoraFim);
            if (ocupado)
            {
                throw new Exception("O profissional já possui um agendamento ou bloqueio neste horário.");
            }

            if (dto.DataHoraFim <= dto.DataHoraInicio)
                throw new Exception("A data de término do bloqueio deve ser posterior à data de início.");

            var bloqueioHorario = new BloqueioHorario
            {
                EmpresaId = dto.EmpresaId,
                ProfissionalId = dto.ProfissionalId,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dto.DataHoraFim,
                Motivo = dto.Motivo,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _repositoryBloqueioHorario.Adicionar(bloqueioHorario);
            return bloqueioHorario;
        }

        public async Task<BloqueioHorario> AtualizarBloqueioHorario(long id, BloqueioHorarioUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os ados não foram preenchidos.");

            var bloqueio = await _repositoryBloqueioHorario.ObterPorId(id);
            if (bloqueio == null)
            {
                throw new Exception($"Bloqueio de horário com id {id} não encontrado.");
            }

            var ocupado = await _repositoryAgendamento.EstaOcupado(bloqueio.ProfissionalId, dto.DataHoraInicio, dto.DataHoraFim, bloqueioIdParaIgnorar: id);
            if (ocupado)
            {
                throw new Exception("O profissional já possui um agendamento ou bloqueio neste horário.");
            }

            if (dto.DataHoraFim <= dto.DataHoraInicio)
                throw new Exception("A data de término do bloqueio deve ser posterior à data de início.");

            bloqueio.DataHoraInicio = dto.DataHoraInicio;
            bloqueio.DataHoraFim = dto.DataHoraFim;
            bloqueio.Motivo = dto.Motivo ?? bloqueio.Motivo;
            bloqueio.DataAtualizacao = DateTime.UtcNow;

            await _repositoryBloqueioHorario.Atualizar(bloqueio);
            return bloqueio;
        }

        public async Task<BloqueioHorario?> DeletarBloqueioHorario(long id)
        {
            var bloqueio = await _repositoryBloqueioHorario.ObterPorId(id);
            if (bloqueio == null)
            {
                throw new Exception($"Bloqueio de horário com id {id} não encontrado.");
            }
            bloqueio.IsDeleted = true;
            await _repositoryBloqueioHorario.Deletar(bloqueio);
            return bloqueio;
        }

        public async Task<IEnumerable<BloqueioHorario>> ObterPorEmpresaId(long empresaId)
        {
            var bloqueio = await _repositoryBloqueioHorario.ObterPorEmpresaId(empresaId);
            if (bloqueio == null)
            {
                throw new Exception($"Empresa com id {empresaId} não tem bloqueios de horario.");
            }
            return bloqueio;
        }
        public async Task<IEnumerable<BloqueioHorario>> ObterPorProfissionalId(long profissionalId)
        {
            var bloqueio = await _repositoryBloqueioHorario.ObterPorProfissionalId(profissionalId);
            if (bloqueio == null)
            {
                throw new Exception($"Profissional com id {profissionalId} não possui bloqueios de horario.");
            }
            return bloqueio;
        }
        #endregion
    }
}
