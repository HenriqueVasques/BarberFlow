using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class AgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IClienteRepository _clienteRepository;

        public AgendamentoService( IAgendamentoRepository agendamentoRepository, IServicoRepository servicoRepository, IProfissionalRepository profissionalRepository, IClienteRepository clienteRepository)
        {
            _agendamentoRepository = agendamentoRepository;
            _servicoRepository = servicoRepository;
            _profissionalRepository = profissionalRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<Agendamento> AdicionarAgendamento(AgendamentoCreateDto dto)
        {
            var servico = await _servicoRepository.ObterPorId(dto.ServicoId)
                ?? throw new Exception($"Serviço ID {dto.ServicoId} não encontrado.");

            var cliente = await _clienteRepository.ObterPorId(dto.ClienteId)
                ?? throw new Exception($"Cliente ID {dto.ClienteId} não encontrado.");

            var profissional = await _profissionalRepository.ObterPorId(dto.ProfissionalId)
                ?? throw new Exception("Profissional não encontrado.");

            var dataFimCalculada = dto.DataHoraInicio.AddMinutes(servico.DuracaoMinutos);

            var ocupado = await _agendamentoRepository.EstaOcupado(dto.ProfissionalId, dto.DataHoraInicio, dataFimCalculada);
            if (ocupado)
            {
                throw new Exception("O profissional já possui um agendamento ou bloqueio neste horário.");
            }

            var agendamento = new Agendamento
            {
                EmpresaId = dto.EmpresaId,
                ProfissionalId = dto.ProfissionalId,
                ClienteId = dto.ClienteId,
                ServicoId = dto.ServicoId,
                PrecoNoMomento = servico.PrecoBase,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dataFimCalculada,
                Status = StatusAgendamento.Pendente,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _agendamentoRepository.Adicionar(agendamento);

            return await _agendamentoRepository.ObterPorId(agendamento.Id)
                ?? throw new Exception("Erro crítico ao recuperar o agendamento após a criação.");
        }

        public async Task<Agendamento> Cancelar(long id)
        {
            var agendamento = await _agendamentoRepository.ObterPorId(id)
                ?? throw new Exception($"Agendamento ID {id} não encontrado.");

            if (agendamento.Status == StatusAgendamento.Cancelado || agendamento.Status == StatusAgendamento.Finalizado)
            {
                throw new Exception("Não é possível cancelar um agendamento que já foi finalizado ou cancelado.");
            }

            agendamento.Status = StatusAgendamento.Cancelado;
            agendamento.DataAtualizacao = DateTime.UtcNow;

            await _agendamentoRepository.Atualizar(agendamento);
            return agendamento;
        }

        public async Task<List<Agendamento>> ObterAgendaPorPeriodo(long profissionalId, long empresaId, DateTime inicio, DateTime fim, List<StatusAgendamento> statusFiltro)
        {
            return await _agendamentoRepository.ObterAgendaPorPeriodo(profissionalId, empresaId, inicio, fim, statusFiltro);
        }

        public async Task<Agendamento> ObterPorId(long id)
        {
            return await _agendamentoRepository.ObterPorId(id)
                ?? throw new Exception($"Agendamento ID {id} não encontrado.");
        }
    }
}