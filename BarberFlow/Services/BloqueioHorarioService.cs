using BarberFlow.API.Controllers;
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
        public BloqueioHorarioService(
            IBloqueioHorarioRepository repositoryBloqueioHorario,
            IAgendamentoRepository repositoryAgendamento,
            IProfissionalRepository repositoryProfissional,
            IEmpresaRepository repositoryEmpresa)
        {
            _repositoryBloqueioHorario = repositoryBloqueioHorario;
            _repositoryAgendamento = repositoryAgendamento;
            _repositoryProfissional = repositoryProfissional;
            _repositoryEmpresa = repositoryEmpresa;
        }
        #endregion

        #region Ações de Escrita (Regras de Negócio)

        // Orquestra a criação de um bloqueio: valida existência de entidades e conflitos de agenda
        public async Task<BloqueioHorarioResponseDto> CriarBloqueioHorario(BloqueioHorarioCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            if (dto.DataHoraFim <= dto.DataHoraInicio)
                throw new Exception("A data de término do bloqueio deve ser posterior à data de início.");

            var empresa = await _repositoryEmpresa.ObterPorId(dto.EmpresaId)
                ?? throw new Exception($"Empresa com id {dto.EmpresaId} não encontrada.");

            var profissional = await _repositoryProfissional.ObterPorId(dto.ProfissionalId)
                ?? throw new Exception($"Profissional com id {dto.ProfissionalId} não encontrado.");

            // Verifica se o horário já está ocupado por agendamentos ou outros bloqueios
            var ocupado = await _repositoryAgendamento.EstaOcupado(profissional.Id, dto.DataHoraInicio, dto.DataHoraFim);
            if (ocupado)
                throw new Exception("O profissional já possui um agendamento, bloqueio ou o horário está fora do expediente.");

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

            return new BloqueioHorarioResponseDto
            {
                Id = bloqueioHorario.Id,
                EmpresaId = bloqueioHorario.EmpresaId,
                ProfissionalId = bloqueioHorario.ProfissionalId,
                DataHoraInicio = bloqueioHorario.DataHoraInicio,
                DataHoraFim = bloqueioHorario.DataHoraFim,
                Motivo = bloqueioHorario.Motivo
            };
        }

        // Atualiza os horários ou motivo de um bloqueio existente, validando novos conflitos
        public async Task AtualizarBloqueioHorario(long id, BloqueioHorarioUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            if (dto.DataHoraFim <= dto.DataHoraInicio)
                throw new Exception("A data de término do bloqueio deve ser posterior à data de início.");

            var bloqueio = await _repositoryBloqueioHorario.ObterPorId(id)
                ?? throw new Exception($"Bloqueio de horário com id {id} não encontrado.");

            // Valida ocupação ignorando o próprio ID do bloqueio que está sendo editado
            var ocupado = await _repositoryAgendamento.EstaOcupado(bloqueio.ProfissionalId, dto.DataHoraInicio, dto.DataHoraFim, bloqueioIdParaIgnorar: id);
            if (ocupado)
                throw new Exception("O profissional já possui um agendamento, bloqueio ou o horário está fora do expediente.");

            bloqueio.DataHoraInicio = dto.DataHoraInicio;
            bloqueio.DataHoraFim = dto.DataHoraFim;
            bloqueio.Motivo = dto.Motivo ?? bloqueio.Motivo;
            bloqueio.DataAtualizacao = DateTime.UtcNow;

            await _repositoryBloqueioHorario.Atualizar(bloqueio);
        }

        // Realiza a exclusão lógica de um bloqueio de horário
        public async Task DeletarBloqueioHorario(long id)
        {
            var bloqueio = await _repositoryBloqueioHorario.ObterPorId(id)
                ?? throw new Exception($"Bloqueio com id {id} não encontrado.");

            bloqueio.IsDeleted = true;
            bloqueio.DataAtualizacao = DateTime.UtcNow;

            await _repositoryBloqueioHorario.Deletar(bloqueio);
        }

        #endregion

        #region Visão: Profissional / Admin (Agenda e Relatórios)

        // Recupera bloqueios de uma empresa específica dentro de um intervalo de datas
        public async Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorEmpresaId(long empresaId, DateOnly inicio, DateOnly fim, int pagina, bool incluirDeletados)
        {
            if (inicio == default || fim == default)
                throw new Exception("As datas de início e fim devem ser informadas.");

            if (inicio > fim)
                throw new Exception("A data de início deve ser anterior ou igual à data de término.");

            if (pagina <= 0) pagina = 1;

            var bloqueios = await _repositoryBloqueioHorario.ObterPorEmpresaId(empresaId, inicio, fim, incluirDeletados, pagina);

            return bloqueios ?? Enumerable.Empty<BloqueioHorarioResponseDto>();
        }

        // Recupera bloqueios de um profissional específico para visualização na agenda pessoal
        public async Task<IEnumerable<BloqueioHorarioResponseDto>> ObterPorProfissionalId(long profissionalId, DateOnly inicio, DateOnly fim, int pagina, bool incluirDeletados)
        {
            if (inicio == default || fim == default)
                throw new Exception("As datas de início e fim devem ser informadas.");

            if (inicio > fim)
                throw new Exception("A data de início deve ser anterior ou igual à data de término.");

            if (pagina <= 0) pagina = 1;

            var bloqueios = await _repositoryBloqueioHorario.ObterPorProfissionalId(profissionalId, inicio, fim, incluirDeletados, pagina);

            return bloqueios ?? Enumerable.Empty<BloqueioHorarioResponseDto>();
        }

        #endregion
    }
}