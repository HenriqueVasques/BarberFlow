using BarberFlow.API.DTOs.ProfissionalServico;
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

        public async Task<ProfissionalServicoResponseDto> CriarProfissionalServico(ProfissionalServicoCreateDto dto)
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
            return MapToResponseDto(profissionalServico);      
        }

        public async Task AtualizarProfissionalServico(long id, ProfissionalServicoUpdateDto dto)
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
        }

        public async Task DeletarProfissionalServico(long id)
        {
            var profissionalServico = await _profissionalServicoRepository.ObterPorId(id)
                ?? throw new Exception("Serviço do Profissional não encontrado.");

            profissionalServico.DataAtualizacao = DateTime.Now;
            profissionalServico.IsDeleted = true;
            profissionalServico.Ativo = false;

            await _profissionalServicoRepository.Deletar(profissionalServico);
        }

        public async Task<ProfissionalServicoResponseDto> ObterPorIdAdmin(long id)
        {
            var profissionalServico = await _profissionalServicoRepository.ObterPorId(id) 
                ?? throw new Exception("Serviço do Profissional não encontrado.");
            return MapToResponseDto(profissionalServico);
        }

        public async Task<ProfissionalServicoResponseDto> ObterPorIdCliente(long id)
        {
            var profissionalServico = await _profissionalServicoRepository.ObterPorId(id)
                ?? throw new Exception("Serviço do Profissional não encontrado.");
            return MapToResponseDto(profissionalServico);
        }

        public async Task<List<ProfissionalServicoResponseDto>> ObterPorProfissionalIdAdmin(long profissionalId)
        {
            return await _profissionalServicoRepository.ObterPorProfissionalId(profissionalId, apenasAtivos: false)
                ?? throw new Exception("Nenhum Serviço desse Profissional foi encontrado.");
        }

        public async Task<List<ProfissionalServicoResponseDto>> ObterPorProfissionalIdCliente(long profissionalId)
        {
            return await _profissionalServicoRepository.ObterPorProfissionalId(profissionalId, apenasAtivos: true)
                ?? throw new Exception("Nenhum Serviço desse Profissional foi encontrado.");
        }

        #region Métodos Auxiliares Privados
        private ProfissionalServicoResponseDto MapToResponseDto(ProfissionalServico profissionalServico, string? nome = null, string? nomeEmpresa = null, string? email = null)
        {
            return new ProfissionalServicoResponseDto
            {
                Id = profissionalServico.Id,
                ProfissionalId = profissionalServico.ProfissionalId,
                ServicoId = profissionalServico.ServicoId,
                NomeServico = nome ?? profissionalServico.Servico.Nome,
                NomeProfissional = nome ?? profissionalServico.Profissional.Usuario.Nome,
                PrecoPersonalizado = profissionalServico.PrecoPersonalizado,
                DuracaoPersonalizadaMinutos = profissionalServico.DuracaoPersonalizadaMinutos,
                DataCriacao = profissionalServico.DataCriacao,
                DataAtualizacao = profissionalServico.DataAtualizacao,
                Ativo = profissionalServico.Ativo
            };
        }
        #endregion
    }
}
