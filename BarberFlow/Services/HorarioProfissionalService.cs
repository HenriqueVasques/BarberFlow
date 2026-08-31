using BarberFlow.API.DTOs.HorarioProfissional;
using BarberFlow.API.Interfaces.IRepository;
using BarberFlow.API.Interfaces.IServices;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class HorarioProfissionalService : IHorarioProfissionalService
    {
        private readonly IHorarioProfissionalRepository _horarioProfissionalRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public HorarioProfissionalService(
            IHorarioProfissionalRepository horarioProfissionalRepository,
            IProfissionalRepository profissionalRepository,
            IEmpresaRepository empresaRepository)
        {
            _horarioProfissionalRepository = horarioProfissionalRepository;
            _profissionalRepository = profissionalRepository;
            _empresaRepository = empresaRepository;
        }

        #region Ações de Escrita (Admin)

        // Cadastra o horário do profissional validando contra as regras da empresa e disponibilidade do dia
        public async Task<HorarioProfissionalResponseDto> CriarHorarioProfissional(HorarioProfissionalCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var profissional = await _profissionalRepository.ObterPorId(dto.ProfissionalId!.Value)
                ?? throw new Exception($"Profissional ID {dto.ProfissionalId} não encontrado.");

            var empresaComHorario = await _empresaRepository.ObterPorIdComHorarioEmpresa(profissional.EmpresaId)
                ?? throw new Exception($"Empresa ID {profissional.EmpresaId} não encontrada.");

            if (profissional.HorariosProfissionais.Any(hp => hp.DiaSemana == dto.DiaSemana && !hp.IsDeleted && hp.Ativo))
                throw new Exception("Já existe um horário cadastrado para este profissional neste dia.");

            var regraEmpresa = empresaComHorario.HorariosFuncionamentoEmpresa
                .FirstOrDefault(hfe => hfe.DiaSemana == dto.DiaSemana && !hfe.IsDeleted && hfe.Ativo && !hfe.EstaFechado)
                ?? throw new Exception("A empresa não abre ou não possui horário configurado para este dia.");

            ValidarIntervalosHorario(dto.HoraInicio!.Value, dto.HoraFim!.Value, dto.HoraInicioAlmoco, dto.HoraFimAlmoco, regraEmpresa);

            var horarioProfissional = new HorarioProfissional
            {
                ProfissionalId = dto.ProfissionalId!.Value,
                EmpresaId = profissional.EmpresaId,
                DiaSemana = dto.DiaSemana!.Value,
                HoraInicio = dto.HoraInicio!.Value,
                HoraFim = dto.HoraFim!.Value,
                HoraInicioAlmoco = dto.HoraInicioAlmoco,
                HoraFimAlmoco = dto.HoraFimAlmoco,
            };

            await _horarioProfissionalRepository.Adicionar(horarioProfissional);

            return MapToResponseDto(horarioProfissional);
        }

        // Atualiza os horários permitindo alteração parcial dos campos
        public async Task AtualizarHorarioProfissional(long id, HorarioProfissionalUpdateDto dto)
        {
            var horario = await _horarioProfissionalRepository.ObterPorId(id)
                ?? throw new Exception("Horário do Profissional não encontrado.");

            var diaFinal = dto.DiaSemana ?? horario.DiaSemana;

            // Verifica duplicidade se estiver mudando o dia
            var jaExisteOutro = horario.Profissional.HorariosProfissionais
                .Any(hp => hp.DiaSemana == diaFinal && hp.Id != id && !hp.IsDeleted && hp.Ativo);

            if (jaExisteOutro)
                throw new Exception($"O profissional já possui um horário cadastrado para {diaFinal}.");

            var empresaComHorario = await _empresaRepository.ObterPorIdComHorarioEmpresa(horario.EmpresaId)
                ?? throw new Exception("Empresa não encontrada.");

            var regraEmpresa = empresaComHorario.HorariosFuncionamentoEmpresa
                .FirstOrDefault(hfe => hfe.DiaSemana == diaFinal && !hfe.IsDeleted && hfe.Ativo && !hfe.EstaFechado)
                ?? throw new Exception("A empresa não abre neste dia.");

            // Consolida horários (Novos ou Mantidos) para validação
            var hInicio = dto.HoraInicio ?? horario.HoraInicio;
            var hFim = dto.HoraFim ?? horario.HoraFim;
            var hAlmocoInicio = dto.HoraInicioAlmoco ?? horario.HoraInicioAlmoco;
            var hAlmocoFim = dto.HoraFimAlmoco ?? horario.HoraFimAlmoco;

            ValidarIntervalosHorario(hInicio, hFim, hAlmocoInicio, hAlmocoFim, regraEmpresa);

            horario.DiaSemana = diaFinal;
            horario.HoraInicio = hInicio;
            horario.HoraFim = hFim;
            horario.HoraInicioAlmoco = hAlmocoInicio;
            horario.HoraFimAlmoco = hAlmocoFim;
            horario.DataAtualizacao = DateTime.UtcNow;

            await _horarioProfissionalRepository.Atualizar(horario);
        }

        // Soft Delete do horário do profissional
        public async Task DeletarHorarioProfissional(long id)
        {
            var horario = await _horarioProfissionalRepository.ObterPorId(id)
               ?? throw new Exception("Horário do Profissional não encontrado.");

            horario.Ativo = false;
            horario.IsDeleted = true;
            horario.DataAtualizacao = DateTime.UtcNow;

            await _horarioProfissionalRepository.Deletar(horario);
        }

        #endregion

        #region Consultas (Leitura)

        public async Task<HorarioProfissionalResponseDto> ObterPorId(long id)
        {
            var horarioProfissional = await _horarioProfissionalRepository.ObterPorId(id)
                ?? throw new Exception("Horário do Profissional não encontrado.");

            return MapToResponseDto(horarioProfissional);
        }

        public async Task<List<HorarioProfissionalResponseDto>> ObterPorProfissionalId(long profissionalId)
        {
            var profissional = await _profissionalRepository.ObterPorId(profissionalId)
                ?? throw new Exception($"Profissional ID {profissionalId} não encontrado.");

            return await _horarioProfissionalRepository.ObterPorProfissionalId(profissionalId);
        }

        #endregion

        #region Métodos Auxiliares Privados

        private static void ValidarIntervalosHorario(TimeOnly inicio, TimeOnly fim, TimeOnly? almocoInicio,TimeOnly? almocoFim, HorarioFuncionamentoEmpresa regraEmpresa)
        {
            if (inicio >= fim)
                throw new Exception("O horário de início deve ser menor que o de término.");

            if (regraEmpresa.HoraAbertura.HasValue && inicio < regraEmpresa.HoraAbertura.Value)
                throw new Exception($"O horário de início ({inicio}) é anterior ao horário de abertura da empresa ({regraEmpresa.HoraAbertura.Value}).");

            if (regraEmpresa.HoraFechamento.HasValue && fim > regraEmpresa.HoraFechamento.Value)
                throw new Exception($"O horário de término ({fim}) ultrapassa o horário de fechamento da empresa ({regraEmpresa.HoraFechamento.Value}).");

            if (almocoInicio.HasValue || almocoFim.HasValue)
            {
                if (!almocoInicio.HasValue || !almocoFim.HasValue)
                    throw new Exception("É necessário informar o início e o fim do horário de almoço.");

                if (almocoInicio.Value >= almocoFim.Value)
                    throw new Exception("O início do almoço deve ser menor que o término.");

                if (almocoInicio.Value < inicio || almocoFim.Value > fim)
                    throw new Exception("O horário de almoço deve estar contido dentro do expediente do profissional.");
            }
        }

        private HorarioProfissionalResponseDto MapToResponseDto(HorarioProfissional horarioProfissional)
        {
            return new HorarioProfissionalResponseDto
            {
                Id = horarioProfissional.Id,
                ProfissionalId = horarioProfissional.ProfissionalId,
                EmpresaId = horarioProfissional.EmpresaId,
                DiaSemana = horarioProfissional.DiaSemana,
                HoraInicio = horarioProfissional.HoraInicio,
                HoraFim = horarioProfissional.HoraFim,
                HoraInicioAlmoco = horarioProfissional.HoraInicioAlmoco,
                HoraFimAlmoco = horarioProfissional.HoraFimAlmoco,
                Ativo = horarioProfissional.Ativo
            };
        }

        #endregion
    }
}