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

        public ProfissionalServicoService(
            IProfissionalServicoRepository profissionalServicoRepository,
            IProfissionalRepository profissionalRepository,
            IServicoRepository servicoRepository)
        {
            _profissionalServicoRepository = profissionalServicoRepository;
            _profissionalRepository = profissionalRepository;
            _servicoRepository = servicoRepository;
        }

        #region Comandos (Escrita)

        // Cadastra uma nova associação de serviço para o profissional com preço/duração customizados
        public async Task<ProfissionalServicoResponseDto> CriarProfissionalServico(ProfissionalServicoCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var profissional = await _profissionalRepository.ObterPorId(dto.ProfissionalId)
                ?? throw new Exception("Profissional não encontrado.");

            var servico = await _servicoRepository.ObterPorId(dto.ServicoId)
                ?? throw new Exception("Serviço não encontrado.");

            // Validação de duplicidade: checa se o profissional já possui este serviço vinculado
            var servicosExistentes = await _profissionalServicoRepository.ObterPorProfissionalId(dto.ProfissionalId, apenasAtivos: false);
            if (servicosExistentes != null && servicosExistentes.Any(ps => ps.ServicoId == dto.ServicoId))
            {
                throw new Exception("Este serviço já está vinculado a este profissional.");
            }

            var profissionalServico = new ProfissionalServico
            {
                ProfissionalId = dto.ProfissionalId,
                ServicoId = dto.ServicoId,
                PrecoPersonalizado = dto.PrecoPersonalizado,
                DuracaoPersonalizadaMinutos = dto.DuracaoPersonalizadaMinutos,
                Ativo = true,
                IsDeleted = false,
                DataAtualizacao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow,
            };

            await _profissionalServicoRepository.Adicionar(profissionalServico);
            return MapToResponseDto(profissionalServico, servico.Nome, profissional.Usuario?.Nome);
        }

        // Atualiza a duração e/ou preço personalizado do serviço oferecido pelo profissional
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

            profissionalServico.DataAtualizacao = DateTime.UtcNow;

            await _profissionalServicoRepository.Atualizar(profissionalServico);
        }

        // Executa o Soft Delete e desativa a oferta do serviço pelo profissional
        public async Task DeletarProfissionalServico(long id)
        {
            var profissionalServico = await _profissionalServicoRepository.ObterPorId(id)
                ?? throw new Exception("Serviço do Profissional não encontrado.");

            profissionalServico.DataAtualizacao = DateTime.UtcNow;
            profissionalServico.IsDeleted = true;
            profissionalServico.Ativo = false;

            await _profissionalServicoRepository.Deletar(profissionalServico);
        }

        #endregion

        #region Consultas (Leitura)

        // Busca o serviço do profissional por ID para gestão do Painel Administrativo (inclui inativos)
        public async Task<ProfissionalServicoResponseDto> ObterPorIdAdmin(long id)
        {
            var profissionalServico = await _profissionalServicoRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception("Serviço do Profissional não encontrado.");

            return MapToResponseDto(profissionalServico);
        }

        // Busca o serviço do profissional por ID para o aplicativo do cliente (apenas registros ativos)
        public async Task<ProfissionalServicoResponseDto> ObterPorIdCliente(long id)
        {
            var profissionalServico = await _profissionalServicoRepository.ObterPorId(id, apenasAtivos: true)
                ?? throw new Exception("Serviço do Profissional não encontrado.");

            return MapToResponseDto(profissionalServico);
        }

        // Lista todos os serviços prestados por um profissional para visão administrativa (inclui inativos)
        public async Task<List<ProfissionalServicoResponseDto>> ObterPorProfissionalIdAdmin(long profissionalId)
        {
            return await _profissionalServicoRepository.ObterPorProfissionalId(profissionalId, apenasAtivos: false)
                ?? throw new Exception("Nenhum Serviço desse Profissional foi encontrado.");
        }

        // Lista os serviços disponíveis prestados por um profissional para exibição no aplicativo do cliente
        public async Task<List<ProfissionalServicoResponseDto>> ObterPorProfissionalIdCliente(long profissionalId)
        {
            return await _profissionalServicoRepository.ObterPorProfissionalId(profissionalId, apenasAtivos: true)
                ?? throw new Exception("Nenhum Serviço desse Profissional foi encontrado.");
        }

        #endregion

        #region Métodos Auxiliares Privados

        // Mapeia a entidade de domínio ProfissionalServico para o DTO de resposta da API
        private ProfissionalServicoResponseDto MapToResponseDto(ProfissionalServico profissionalServico, string? nomeServico = null, string? nomeProfissional = null, string? nomeEmpresa = null, string? email = null)
        {
            return new ProfissionalServicoResponseDto
            {
                Id = profissionalServico.Id,
                ProfissionalId = profissionalServico.ProfissionalId,
                ServicoId = profissionalServico.ServicoId,
                NomeServico = nomeServico ?? profissionalServico.Servico?.Nome ?? string.Empty,
                NomeProfissional = nomeProfissional ?? profissionalServico.Profissional?.Usuario?.Nome ?? string.Empty,
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