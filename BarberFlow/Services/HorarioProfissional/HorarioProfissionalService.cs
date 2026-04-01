using BarberFlow.API.DTOs;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Services
{
    public class HorarioProfissionalService
    {
        private readonly IHorarioProfissionalRepository _horarioProfissionalRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public HorarioProfissionalService(IHorarioProfissionalRepository horarioProfissionalRepository, IProfissionalRepository profissionalRepository, IEmpresaRepository empresaRepository)
        {
            _horarioProfissionalRepository = horarioProfissionalRepository;
            _profissionalRepository = profissionalRepository;
            _empresaRepository = empresaRepository;
        }

        public async Task<HorarioProfissional> AdicionarHorarioProfissional(HorarioProfissionalCreateDto dto)
        {

            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var profissional = await _profissionalRepository.ObterPorId(dto.ProfissionalId)
             ?? throw new Exception($"Profiossional ID {dto.ProfissionalId} não encontrado.");

            var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId) 
                ?? throw new Exception($"Empresa ID {dto.EmpresaId} não encontrado.");

            if(profissional.EmpresaId != empresa.Id)
                throw new Exception($"O Profissional com ID {dto.ProfissionalId} não pertence a empresa com Id:{dto.EmpresaId}.");

            if (profissional.HorariosProfissionais.Any(hp => hp.DiaSemana == dto.DiaSemana && !hp.IsDeleted && hp.Ativo))
                throw new Exception("Já existe um horário cadastrado para este profissional neste dia.");

            var horarioFuncionamentoEmpresa = empresa.HorariosFuncionamentoEmpresa
                .FirstOrDefault(hfe => hfe.DiaSemana == dto.DiaSemana && !hfe.IsDeleted && hfe.Ativo);

            if (horarioFuncionamentoEmpresa == null)
                throw new Exception("A empresa não abre neste dia.");

            if (dto.HoraInicio < horarioFuncionamentoEmpresa.HoraAbertura || dto.HoraFim > horarioFuncionamentoEmpresa.HoraFechamento)
                throw new Exception("O horário do profissional excede o funcionamento da empresa.");

            if (dto.HoraInicioAlmoco < horarioFuncionamentoEmpresa.HoraAbertura || dto.HoraFimAlmoco > horarioFuncionamentoEmpresa.HoraFechamento)
                throw new Exception("O horário de almoço do profissional excede o funcionamento da empresa.");

            if (dto.HoraInicio >= dto.HoraFim)
                throw new Exception("O horário de início deve ser menor que o horário de término.");

            if (dto.HoraInicioAlmoco >= dto.HoraFimAlmoco)
                throw new Exception("O início do almoço deve ser menor que o término do almoço.");

            var horarioProfissional  = new HorarioProfissional
            {
                ProfissionalId = dto.ProfissionalId,
                EmpresaId = dto.EmpresaId,
                DiaSemana = dto.DiaSemana,
                HoraInicio = dto.HoraInicio,
                HoraFim = dto.HoraFim,
                HoraInicioAlmoco = dto.HoraInicioAlmoco,
                HoraFimAlmoco = dto.HoraFimAlmoco,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _horarioProfissionalRepository.Adicionar(horarioProfissional);

            return await _horarioProfissionalRepository.ObterPorId(horarioProfissional.Id)
                ?? throw new Exception("Erro ao carregar o objeto");
        }

        public async Task<HorarioProfissional> AtualizarHorarioProfissional(long id, HorarioProfissionalUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var horarioProfissional = await _horarioProfissionalRepository.ObterPorId(id)
                ?? throw new Exception("Horario do Profissional não foi encontrado.");

            var jaExisteOutro = horarioProfissional.Profissional.HorariosProfissionais
            .Any(hp => hp.DiaSemana == dto.DiaSemana && hp.Id != id && !hp.IsDeleted && hp.Ativo);

            if (jaExisteOutro)
                throw new Exception($"O profissional já possui um horário cadastrado para {dto.DiaSemana}.");

            var empresa = await _empresaRepository.ObterPorId(horarioProfissional.EmpresaId)
                ?? throw new Exception($"Empresa ID {horarioProfissional.EmpresaId} não encontrado.");

            var horarioFuncionamentoEmpresa = empresa.HorariosFuncionamentoEmpresa
                .FirstOrDefault(hfe => hfe.DiaSemana == dto.DiaSemana && !hfe.IsDeleted && hfe.Ativo);

            if (horarioFuncionamentoEmpresa == null)
                throw new Exception("A empresa não abre neste dia.");

            if (dto.HoraInicio < horarioFuncionamentoEmpresa.HoraAbertura || dto.HoraFim > horarioFuncionamentoEmpresa.HoraFechamento)
                throw new Exception("O horário do profissional excede o funcionamento da empresa.");

            if (dto.HoraInicioAlmoco < horarioFuncionamentoEmpresa.HoraAbertura || dto.HoraFimAlmoco > horarioFuncionamentoEmpresa.HoraFechamento)
                throw new Exception("O horário de almoço do profissional excede o funcionamento da empresa.");

            if (dto.HoraInicio >= dto.HoraFim)
                throw new Exception("O horário de início deve ser menor que o horário de término.");

            if (dto.HoraInicioAlmoco >= dto.HoraFimAlmoco)
                throw new Exception("O início do almoço deve ser menor que o término do almoço.");

            horarioProfissional.DiaSemana = dto.DiaSemana ?? horarioProfissional.DiaSemana;
            horarioProfissional.HoraInicio = dto.HoraInicio ?? horarioProfissional.HoraInicio;
            horarioProfissional.HoraFim = dto.HoraFim ?? horarioProfissional.HoraFim;
            horarioProfissional.HoraInicioAlmoco = dto.HoraInicioAlmoco ?? horarioProfissional.HoraInicioAlmoco;
            horarioProfissional.HoraFimAlmoco = dto.HoraFimAlmoco ?? horarioProfissional.HoraFimAlmoco;
            horarioProfissional.DataAtualizacao = DateTime.UtcNow;

            await _horarioProfissionalRepository.Atualizar(horarioProfissional);    
            return horarioProfissional;
        }

        public async Task<HorarioProfissional> DeletarHorarioProfissional(long id)
        {
            var horarioProfissional = await _horarioProfissionalRepository.ObterPorId(id)
               ?? throw new Exception("Horario do Profissional não foi encontrado.");

            horarioProfissional.Ativo = false;
            horarioProfissional.IsDeleted = true;

            await _horarioProfissionalRepository.Deletar(horarioProfissional);
            return horarioProfissional;

        }

        public async Task<HorarioProfissional> ObterPorId(long id)
        {
            var horarioProfissional = await _horarioProfissionalRepository.ObterPorId(id)
                ?? throw new Exception("Horario do Profissional não foi encontrado.");
            return horarioProfissional;
        }

        public async Task<List<HorarioProfissional>> ObterPorProfissionalId(long profissionalId)
        {
            var profissional = await _profissionalRepository.ObterPorId(profissionalId)
             ?? throw new Exception($"Profiossional ID {profissionalId} não encontrado.");

            var horarios = await _horarioProfissionalRepository.ObterPorProfissionalId(profissionalId);
            return horarios;
        }
    }
}

