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

        // Cria uma nova configuração de horário validando a empresa e a coerência do intervalo de horas
        public async Task<HorarioFuncionamentoEmpresaResponseDto> CriarHorarioFuncionamentoEmpresa(HorarioFuncionamentoEmpresaCreateDto dto, long empresaId)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var empresa = await _empresaRepository.ObterPorId(empresaId)
                ?? throw new Exception($"Empresa com id {empresaId} não encontrada.");

            if (!dto.EstaFechado && dto.HoraAbertura >= dto.HoraFechamento)
                throw new Exception("A hora de abertura precisa ser menor que a hora de fechamento.");

            var horarioFuncionamentoEmpresa = new HorarioFuncionamentoEmpresa
            {
                EmpresaId = empresaId,
                DiaSemana = dto.DiaSemana,
                HoraAbertura = dto.HoraAbertura,
                HoraFechamento = dto.HoraFechamento,
                EstaFechado = dto.EstaFechado,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _horarioFuncionamentoEmpresaRepository.Adicionar(horarioFuncionamentoEmpresa);

            return new HorarioFuncionamentoEmpresaResponseDto
            {
                Id = horarioFuncionamentoEmpresa.Id,
                EmpresaId = horarioFuncionamentoEmpresa.EmpresaId,
                DiaSemana = horarioFuncionamentoEmpresa.DiaSemana,
                HoraAbertura = horarioFuncionamentoEmpresa.HoraAbertura,
                HoraFechamento = horarioFuncionamentoEmpresa.HoraFechamento,
                Ativo = horarioFuncionamentoEmpresa.Ativo,
                EstaFechado = horarioFuncionamentoEmpresa.EstaFechado
            };
        }

        // Atualiza horários existentes tratando campos nulos como manutenção do valor atual
        public async Task AtualizarHorarioFuncionamentoEmpresa(HorarioFuncionamentoEmpresaUpadteDto dto, long id)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var horario = await _horarioFuncionamentoEmpresaRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception($"Horário com id {id} não encontrado.");

            var aberturaFinal = dto.HoraAbertura ?? horario.HoraAbertura;
            var fechamentoFinal = dto.HoraFechamento ?? horario.HoraFechamento;

            if (!dto.EstaFechado && aberturaFinal >= fechamentoFinal)
                throw new Exception("A hora de abertura precisa ser menor que a hora de fechamento.");

            horario.DiaSemana = dto.DiaSemana;
            horario.HoraAbertura = dto.HoraAbertura ?? horario.HoraAbertura;
            horario.HoraFechamento = dto.HoraFechamento ?? horario.HoraFechamento;
            horario.EstaFechado = dto.EstaFechado;
            horario.DataAtualizacao = DateTime.UtcNow;

            await _horarioFuncionamentoEmpresaRepository.Atualizar(horario);
        }

        // Realiza o descarte lógico (Soft Delete) inativando a configuração no sistema
        public async Task DeletarHorarioFuncionamentoEmpresa(long id)
        {
            var horario = await _horarioFuncionamentoEmpresaRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception($"Horário com id {id} não encontrado.");

            horario.IsDeleted = true;
            horario.Ativo = false;
            horario.DataAtualizacao = DateTime.UtcNow;

            await _horarioFuncionamentoEmpresaRepository.Deletar(horario);
        }

        #endregion

        #region Consultas e Visões

        // Busca configuração de um dia específico para validar disponibilidade no App
        public async Task<HorarioFuncionamentoEmpresaResponseDto?> ObterPorDia(long empresaId, DayOfWeek diaDaSemana, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _horarioFuncionamentoEmpresaRepository.ObterPorDia(empresaId, diaDaSemana, apenasAtivos, incluirDeletados);
        }

        // Retorna todos os horários de uma empresa projetados para ResponseDto
        public async Task<List<HorarioFuncionamentoEmpresaResponseDto>> ObterPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _horarioFuncionamentoEmpresaRepository.ObterTodosPorEmpresa(empresaId, apenasAtivos, incluirDeletados);
        }

        // Recupera a model completa por ID para operações internas ou edição
        public async Task<HorarioFuncionamentoEmpresa> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _horarioFuncionamentoEmpresaRepository.ObterPorId(id, apenasAtivos, incluirDeletados)
                ?? throw new Exception($"Configuração de horário com id {id} não encontrada.");
        }

        #endregion
    }
}