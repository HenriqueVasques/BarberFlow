using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class AgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IProfissionalServicoRepository _profissionalServicoRepository;
        private readonly IClienteRepository _clienteRepository;

        public AgendamentoService(
            IAgendamentoRepository agendamentoRepository,
            IProfissionalServicoRepository profissionalservicoRepository,
            IClienteRepository clienteRepository)
        {
            _agendamentoRepository = agendamentoRepository;
            _profissionalServicoRepository = profissionalservicoRepository;
            _clienteRepository = clienteRepository;
        }

        #region Ações de Escrita (Regras de Negócio) 

        // Orquestra a criação do agendamento: valida dados, calcula horários e verifica disponibilidade
        public async Task<AgendamentoResponseDto> AdicionarAgendamento(AgendamentoCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var profissionalServico = await _profissionalServicoRepository.ObterPorId(dto.ProfissionalServicoId)
                ?? throw new Exception($"Serviço ID {dto.ProfissionalServicoId} não encontrado.");

            var cliente = await _clienteRepository.ObterPorId(dto.ClienteId)
                ?? throw new Exception($"Cliente ID {dto.ClienteId} não encontrado.");

            var dataFimCalculada = dto.DataHoraInicio.AddMinutes(profissionalServico.DuracaoPersonalizadaMinutos ?? profissionalServico.Servico.DuracaoMinutos);

            var ocupado = await _agendamentoRepository.EstaOcupado(profissionalServico.ProfissionalId, dto.DataHoraInicio, dataFimCalculada);
            if (ocupado)
            {
                throw new Exception("O profissional já possui um agendamento, bloqueio ou ainda, não trabalha neste horário.");
            }   

            var agendamento = new Agendamento
            {
                EmpresaId = dto.EmpresaId,
                ClienteId = dto.ClienteId,
                ProfissionalServicoId = dto.ProfissionalServicoId,
                PrecoNoMomento = profissionalServico.PrecoPersonalizado ?? profissionalServico.Servico.PrecoBase,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dataFimCalculada,
                Status = StatusAgendamento.Pendente,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _agendamentoRepository.Adicionar(agendamento);

            var agendamentoRecuperado = await _agendamentoRepository.ObterPorId(agendamento.Id)
                ?? throw new Exception("Erro crítico ao recuperar o agendamento após a criação.");

            return MapearParaResponseDto(agendamentoRecuperado);
        }

        // Valida e processa o cancelamento de um agendamento ativo
        public async Task Cancelar(long id)
        {
            var agendamento = await _agendamentoRepository.ObterPorId(id)
                ?? throw new Exception($"Agendamento ID {id} não encontrado.");

            if (agendamento.Status == StatusAgendamento.Cancelado || agendamento.Status == StatusAgendamento.Finalizado)
                throw new Exception("Não é possível cancelar um agendamento que já foi finalizado ou cancelado.");

            agendamento.Status = StatusAgendamento.Cancelado;
            agendamento.DataAtualizacao = DateTime.UtcNow;

            await _agendamentoRepository.Atualizar(agendamento);
        }

        // Conclui o atendimento e atualiza o registro no banco
        public async Task Finalizar(long id)
        {
            var agendamento = await _agendamentoRepository.ObterPorId(id)
                ?? throw new Exception($"Agendamento ID {id} não encontrado.");

            if (agendamento.Status == StatusAgendamento.Cancelado || agendamento.Status == StatusAgendamento.Finalizado)
                throw new Exception("Não é possível finalizar um agendamento que já foi finalizado ou cancelado.");

            agendamento.Status = StatusAgendamento.Finalizado;
            agendamento.DataAtualizacao = DateTime.UtcNow;

            await _agendamentoRepository.Atualizar(agendamento);
        }

        #endregion

        #region Visão: Geral

        // Recupera um agendamento específico via repositório
        public async Task<AgendamentoResponseDto> ObterPorId(long id)
        {
            var agendamento =  await _agendamentoRepository.ObterPorId(id)
                ?? throw new Exception($"Agendamento ID {id} não encontrado.");

            return MapearParaResponseDto(agendamento);
        }

        #endregion

        #region Visão: Cliente

        // Consulta o repositório para buscar o próximo compromisso na agenda do cliente
        public async Task<AgendamentoResponseDto?> ObterProximoAgendamentoCliente(long clienteId)
        {
            return await _agendamentoRepository.ObterProximoAgendamentoCliente(clienteId);
        }

        // Obtém a lista histórica dos últimos atendimentos do cliente
        public async Task<List<AgendamentoResponseDto>> ObterUltimosAgendamentosPorCliente(long clienteId)
        {
            if (clienteId <= 0)
                throw new Exception("ID do cliente inválido.");

            return await _agendamentoRepository.ObterUltimosAgendamentosPorCliente(clienteId, 10);
        }

        #endregion

        #region Visão: Profissional / Admin (Agenda e Relatórios)

        // Recupera a lista de agendamentos filtrada por período e status
        public async Task<List<AgendamentoResponseDto>> ObterAgendaPorPeriodo(long? profissionalId, long empresaId, DateOnly inicio, DateOnly fim, List<StatusAgendamento> statusFiltro)
        {
            if (inicio == default)
                throw new Exception("A data inicial precisa ser preenchida.");

            if (fim == default)
                throw new Exception("A data final precisa ser preenchida.");

            if (inicio > fim)
                throw new Exception("A data final precisa ser maior que a inicial.");

            if ((fim.DayNumber - inicio.DayNumber) > 365)
                throw new Exception("O período máximo de consulta é de 1 ano.");

            return await _agendamentoRepository.ObterAgendaPorPeriodo(profissionalId, empresaId, inicio, fim, statusFiltro);
        }

        // Consolida os dados financeiros e de volume para o resumo diário do dashboard
        public async Task<DashboardResumoDto> ObterResumoPorDia(long empresaId, DateOnly data)
        {
            if (data == default)
                throw new Exception("A data precisa ser preenchida.");

            var hoje = DateOnly.FromDateTime(DateTime.Today);
            if (data > hoje)
                throw new Exception("A data não pode ser maior que o dia de hoje.");

            var totalFaturamento = await _agendamentoRepository.ObterFaturamentoPorDia(empresaId, data);
            var quantidadeAtendimentos = await _agendamentoRepository.ContarAgendamentoPorDia(empresaId, data);

            return new DashboardResumoDto
            {
                Data = data,
                FaturamentoTotal = totalFaturamento,
                QuantidadeAtendimentos = quantidadeAtendimentos
            };
        }

        #endregion

        #region Métodos Auxiliares (Private)

        private static AgendamentoResponseDto MapearParaResponseDto(Agendamento agendamento)
        {
            return new AgendamentoResponseDto
            {
                Id = agendamento.Id,
                EmpresaId = agendamento.EmpresaId,
                ClienteId = agendamento.ClienteId,
                ProfissionalServicoId = agendamento.ProfissionalServicoId,
                PrecoNoMomento = agendamento.PrecoNoMomento,
                DataHoraInicio = agendamento.DataHoraInicio,
                DataHoraFim = agendamento.DataHoraFim,
                Status = agendamento.Status,
                DataCriacao = agendamento.DataCriacao,
                DataAtualizacao = agendamento.DataAtualizacao
            };
        }

        #endregion
    }
}