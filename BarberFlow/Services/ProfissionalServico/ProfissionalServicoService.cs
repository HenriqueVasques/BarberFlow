using BarberFlow.API.Data.Repositories;
using BarberFlow.API.DTOs;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class ProfissionalServicoService
    {
        private readonly IProfissionalServicoRepository _profissionalServicoRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IServicoRepository _servicoRepository;

        public ProfissionalServicoService(IProfissionalServicoRepository profissionalServicoRepository, IProfissionalRepository profissionalRepository, IServicoRepository servicoRepository)
        {
            _profissionalServicoRepository = profissionalServicoRepository;
            _profissionalRepository = profissionalRepository;
            _servicoRepository = servicoRepository;
        }

        public async Task<ProfissionalServico> CriarProfissionalServico(ProfissionalServicoCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var profissional = await _profissionalRepository.ObterPorId(dto.ProfissionalId)
                ?? throw new Exception("Profissional não encontrado.");

            var servico = await _servicoRepository.ObterPorId(dto.ServicoId)
                ?? throw new Exception("Serviço não encontrado.");

            var profissionalServico = new ProfissionalServico
            {
                ProfissionalId = dto.ProfissionalId,
                ServicoId = dto.ServicoId,
                PrecoPersonalizado = dto.PrecoPersonalizado,
                DuracaoPersonalizadaMinutos = dto.DuracaoPersonalizadaMinutos,
                Ativo = true,
                IsDeleted = false,
                DataAtualizacao = DateTime.Now,
                DataCriacao = DateTime.Now,
            };

            await _profissionalServicoRepository.Adicionar(profissionalServico);

            return await _profissionalServicoRepository.ObterPorId(profissionalServico.Id)
               ?? throw new Exception("Erro crítico ao recuperar o serviço do profissional após a criação.");
        }

        public async Task<ProfissionalServico> AtualizarProfissionalServico(long id, ProfissionalServicoUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var profissionalServico = await _profissionalServicoRepository.ObterPorId(id)
                ?? throw new Exception("Serviço do Profissional não encontrado.");

            if (dto.DuracaoPersonalizadaMinutos != null)
                profissionalServico.DuracaoPersonalizadaMinutos = dto.DuracaoPersonalizadaMinutos;

            if (dto.PrecoPersonalizado != null)
                profissionalServico.PrecoPersonalizado = dto.PrecoPersonalizado;

            profissionalServico.DataAtualizacao = DateTime.Now;

            await _profissionalServicoRepository.Atualizar(profissionalServico);
            return profissionalServico;
        }

        public async Task<ProfissionalServico> DeletarProfissionalServico(long id)
        {
            var profissionalServico = await _profissionalServicoRepository.ObterPorId(id)
                ?? throw new Exception("Serviço do Profissional não encontrado.");

            profissionalServico.DataAtualizacao = DateTime.Now;
            profissionalServico.IsDeleted = true;
            profissionalServico.Ativo = false;

            await _profissionalServicoRepository.Deletar(profissionalServico);
            return profissionalServico;
        }

        public async Task<ProfissionalServico> ObterPorIdAdmin(long id)
        {
            return await _profissionalServicoRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception("Esse Serviço não foi encontrado.");
        }

        public async Task<ProfissionalServico> ObterPorIdCliente(long id)
        {
            return await _profissionalServicoRepository.ObterPorId(id, apenasAtivos: true)
                ?? throw new Exception("Esse Serviço não foi encontrado.");
        }

        public async Task<List<ProfissionalServico>> ObterPorProfissionalIdAdmin(long profissionalId)
        {
            return await _profissionalServicoRepository.ObterPorProfissionalId(profissionalId, apenasAtivos: false)
                ?? throw new Exception("Nenhum Serviço desse Profissional foi encontrado.");
        }

        public async Task<List<ProfissionalServico>> ObterPorProfissionalIdCliente(long profissionalId)
        {
            return await _profissionalServicoRepository.ObterPorProfissionalId(profissionalId, apenasAtivos: true)
                ?? throw new Exception("Nenhum Serviço desse Profissional foi encontrado.");
        }

    }
}
