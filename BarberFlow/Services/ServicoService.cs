using BarberFlow.API.DTOs.Servico;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class ServicoService
    {
        private readonly IServicoRepository _servicoRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public ServicoService(IServicoRepository servicoRepository, IEmpresaRepository empresaRepository)
        {
            _servicoRepository = servicoRepository;
            _empresaRepository = empresaRepository;
        }

        #region Comandos (Escrita)

        // Cadastra um novo serviço base associado a uma empresa
        public async Task<ServicoResponseDto> CriarServico(ServicoCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId)
                ?? throw new Exception($"Empresa com ID {dto.EmpresaId} não encontrada.");

            // Validação de duplicidade: checa se a empresa já possui um serviço cadastrado com este nome
            var servicosExistentes = await _servicoRepository.ObterPorEmpresa(dto.EmpresaId, apenasAtivos: false, incluirDeletados: false);
            if (servicosExistentes != null && servicosExistentes.Any(s => s.Nome.Trim().Equals(dto.Nome.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Já existe um serviço cadastrado com o nome '{dto.Nome}' nesta empresa.");
            }

            var servico = new Servico
            {
                Nome = dto.Nome,
                DuracaoMinutos = dto.DuracaoMinutos,
                PrecoBase = dto.PrecoBase,
                EmpresaId = dto.EmpresaId,
                Ativo = true,
                IsDeleted = false,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _servicoRepository.Adicionar(servico);

            return MapToResponseDto(servico);
        }

        // Atualiza as informações bases do serviço (nome, duração e preço)
        public async Task AtualizarServico(long id, ServicoUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var servico = await _servicoRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception($"Serviço com ID {id} não foi encontrado.");

            // Caso o nome esteja sendo alterado, valida duplicidade em relação aos outros serviços da mesma empresa
            if (!servico.Nome.Trim().Equals(dto.Nome.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                var servicosEmpresa = await _servicoRepository.ObterPorEmpresa(servico.EmpresaId, apenasAtivos: false, incluirDeletados: false);
                if (servicosEmpresa != null && servicosEmpresa.Any(s => s.Id != id && s.Nome.Trim().Equals(dto.Nome.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception($"Já existe outro serviço cadastrado com o nome '{dto.Nome}' nesta empresa.");
                }
            }

            servico.Nome = dto.Nome;
            servico.DuracaoMinutos = dto.DuracaoMinutos;
            servico.PrecoBase = dto.PrecoBase;
            servico.DataAtualizacao = DateTime.UtcNow;

            await _servicoRepository.Atualizar(servico);
        }

        // Aplica o Soft Delete e desativa o serviço no catálogo geral da empresa
        public async Task DeletarServico(long id)
        {
            var servico = await _servicoRepository.ObterPorId(id, apenasAtivos: false)
                ?? throw new Exception($"Serviço com ID {id} não foi encontrado.");

            servico.IsDeleted = true;
            servico.Ativo = false;
            servico.DataAtualizacao = DateTime.UtcNow;

            await _servicoRepository.Deletar(servico);
        }

        #endregion

        #region Consultas (Leitura)

        // Traz a lista completa de serviços da empresa para gestão do Painel Administrativo (inclui inativos)
        public async Task<IEnumerable<ServicoResponseDto>> ObterServicosPorEmpresaAdmin(long empresaId)
        {
            var empresa = await _empresaRepository.ObterPorId(empresaId)
                ?? throw new Exception($"Empresa com ID {empresaId} não encontrada.");

            return await _servicoRepository.ObterPorEmpresa(empresaId, apenasAtivos: false, incluirDeletados: false);
        }

        // Traz apenas os serviços ativos da empresa para catálogo de agendamentos no App do Cliente
        public async Task<IEnumerable<ServicoResponseDto>> ObterServicosPorEmpresaCliente(long empresaId)
        {
            var empresa = await _empresaRepository.ObterPorId(empresaId)
                ?? throw new Exception($"Empresa com ID {empresaId} não encontrada.");

            return await _servicoRepository.ObterPorEmpresa(empresaId, apenasAtivos: true, incluirDeletados: false);
        }

        #endregion

        #region Métodos Auxiliares Privados

        // Mapeia a entidade de domínio Servico para o DTO de resposta da API
        private static ServicoResponseDto MapToResponseDto(Servico servico)
        {
            return new ServicoResponseDto
            {
                Id = servico.Id,
                Nome = servico.Nome,
                NomeEmpresa = servico.Empresa?.Nome ?? string.Empty,
                DuracaoMinutos = servico.DuracaoMinutos,
                PrecoBase = servico.PrecoBase,
                DataCriacao = servico.DataCriacao,
                DataAtualizacao = servico.DataAtualizacao,
                Ativo = servico.Ativo
            };
        }

        #endregion
    }
}