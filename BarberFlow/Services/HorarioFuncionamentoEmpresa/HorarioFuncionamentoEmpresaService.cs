using BarberFlow.API.DTOs;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class HorarioFuncionamentoEmpresaService
    {
        private readonly IHorarioFuncionamentoEmpresaRepository _horarioFuncionamentoEmpresaRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public HorarioFuncionamentoEmpresaService(
            IHorarioFuncionamentoEmpresaRepository horarioFuncionamentoEmpresaRepository,
            IEmpresaRepository empresaRepository)
        {
            _horarioFuncionamentoEmpresaRepository = horarioFuncionamentoEmpresaRepository;
            _empresaRepository = empresaRepository;
        }

        #region Ações de Escrita (Admin)

        // Cria uma nova configuração de horário validando a existência da empresa e coerência das horas
        public async Task<HorarioFuncionamentoEmpresa> CriarHorarioFuncionamentoEmpresa(HorarioFuncionamentoEmpresaCreateDto dto, long empresaId)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var empresa = await _empresaRepository.ObterPorId(empresaId)
                ?? throw new Exception($"Empresa com id {empresaId} não encontrada.");

            // Validação: Abertura deve ser antes do fechamento
            if (dto.HoraAbertura >= dto.HoraFechamento && !dto.EstaFechado)
                throw new Exception("A hora de abertura precisa ser menor que a hora de fechamento.");

            var horarioFuncionamentoEmpresa = new HorarioFuncionamentoEmpresa
            {
                EmpresaId = empresaId,
                DiaSemana = dto.DiaSemana,
                HoraAbertura = dto.HoraAbertura,
                HoraFechamento = dto.HoraFechamento,
                EstaFechado = dto.EstaFechado,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow,
            };

            await _horarioFuncionamentoEmpresaRepository.Adicionar(horarioFuncionamentoEmpresa);
            return horarioFuncionamentoEmpresa;
        }

        // Atualiza horários existentes permitindo nulidade nos campos não alterados
        public async Task<HorarioFuncionamentoEmpresa> AtualizarHorarioFuncionamentoEmpresa(HorarioFuncionamentoEmpresaUpadteDto dto, long id)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var horario = await _horarioFuncionamentoEmpresaRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception($"Horário com id {id} não encontrado.");

            if (dto.HoraAbertura >= dto.HoraFechamento && !dto.EstaFechado)
                throw new Exception("A hora de abertura precisa ser menor que a hora de fechamento.");

            horario.DiaSemana = dto.DiaSemana;
            horario.HoraAbertura = dto.HoraAbertura ?? horario.HoraAbertura;
            horario.HoraFechamento = dto.HoraFechamento ?? horario.HoraFechamento;
            horario.EstaFechado = dto.EstaFechado;
            horario.DataAtualizacao = DateTime.UtcNow;

            await _horarioFuncionamentoEmpresaRepository.Atualizar(horario);
            return horario;
        }

        // Realiza o descarte lógico (Soft Delete) da configuração
        public async Task<HorarioFuncionamentoEmpresa> DeletarHorarioFuncionamentoEmpresa(long id)
        {
            var horario = await _horarioFuncionamentoEmpresaRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception($"Horário com id {id} não encontrado.");

            await _horarioFuncionamentoEmpresaRepository.Deletar(horario);
            return horario;
        }

        #endregion

        #region Visão: Cliente (App)

        // Retorna apenas horários ativos e não deletados para o processo de agendamento
        public async Task<List<HorarioFuncionamentoEmpresa>> ObterTodosPorEmpresaCliente(long empresaId)
        {
            return await _horarioFuncionamentoEmpresaRepository.ObterTodosPorEmpresa(empresaId, apenasAtivos: true);
        }

        // Busca configuração de um dia específico para validar disponibilidade no App
        public async Task<HorarioFuncionamentoEmpresa?> ObterPorDiaParaCliente(long empresaId, DayOfWeek diaDaSemana)
        {
            return await _horarioFuncionamentoEmpresaRepository.ObterPorDia(empresaId, diaDaSemana, apenasAtivos: true);
        }

        #endregion

        #region Visão: Admin (Painel de Controle)

        // Retorna todos os horários (incluindo inativos) para gestão no dashboard
        public async Task<List<HorarioFuncionamentoEmpresa>> ObterTodosPorEmpresaParaAdmin(long empresaId)
        {
            return await _horarioFuncionamentoEmpresaRepository.ObterTodosPorEmpresa(empresaId, apenasAtivos: false);
        }

        // Recupera um horário específico pelo ID para edição no painel administrativo
        public async Task<HorarioFuncionamentoEmpresa> ObterPorIdAdmin(long id)
        {
            return await _horarioFuncionamentoEmpresaRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception($"Configuração de horário com id {id} não encontrada.");
        }

        #endregion
    }
}