using BarberFlow.API.Data.Repositories;
using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class AgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IUsuarioRepository _usuarioRepository;


        public AgendamentoService(IAgendamentoRepository agendamentoRepository, IServicoRepository servicoRepository, IUsuarioRepository usuarioRepository, IProfissionalRepository profissionalRepository)
        {
            _agendamentoRepository = agendamentoRepository;
            _servicoRepository = servicoRepository;
        }

        public async Task<Agendamento> AdicionarAgendamento(AgendamentoCreateDto dto)
        {
            var servico = await _servicoRepository.ObterPorId(dto.ServicoId);
            if (servico == null)
            {
                throw new Exception($"Serviço com id {dto.ServicoId} não encontrado.");
            }

            var cliente = await _usuarioRepository.ObterPorId(dto.ClienteId);
            if (cliente == null) throw new Exception($"Cliente com ID {dto.ClienteId} não encontrado.");

            // 3. Validar Profissional
            var profissional = await _profissionalRepository.ObterPorId(dto.ProfissionalId);
            if (profissional == null) throw new Exception("Profissional não encontrado.");

            var dataFimCalculada = dto.DataHoraInicio.AddMinutes(servico.DuracaoMinutos);

            var conflito = await _agendamentoRepository.EstaOcupado(dto.ProfissionalId, dto.DataHoraInicio, dataFimCalculada);
            if (conflito)
            {
                throw new Exception("O profissional não está disponível nesse horário.");
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
                Status = Enums.StatusAgendamento.Pendente,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _agendamentoRepository.Adicionar(agendamento);
            return agendamento;
        }

        public async Task Cancelar(long id)
        {
            var agendamento = await _agendamentoRepository.ObterPorId(id);
            if (agendamento == null)
            {
                throw new Exception($"Agendamento com id {id} não encontrado.");
            }

            if (agendamento.Status == Enums.StatusAgendamento.Cancelado || agendamento.Status == Enums.StatusAgendamento.Concluido)
            {
                throw new Exception("Não é possível alterar o status de um agendamento cancelado ou finalizado.");
            }
            agendamento.Status = Enums.StatusAgendamento.Cancelado;
            agendamento.DataAtualizacao = DateTime.UtcNow;
            await _agendamentoRepository.Atualizar(agendamento);
        }

        public async Task<List<Agendamento>> ObterAgendaProfissionalPorData(long profissionalId, long empresaId, DateTime data)
        {
            var agenda = await _agendamentoRepository.ObterAgendaProfissionalPorData(profissionalId, empresaId, data);
            if (agenda == null || !agenda.Any())
            {
                throw new Exception($"Nenhum agendamento encontrado para o profissional {profissionalId} na data {data.ToShortDateString()}.");
            }
            
            return agenda;
        }
    }
}
