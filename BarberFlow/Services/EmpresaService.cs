using BarberFlow.API.DTOs.Empresa;
using BarberFlow.API.Helpers;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class EmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;

        public EmpresaService(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        #region Operações de Escrita (Ações de Comando)

        // Realiza a validação de CNPJ, gera um slug único com base no nome e cadastra a nova empresa
        public async Task<EmpresaResponseDto> CriarEmpresa(EmpresaCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            if (await _empresaRepository.ExisteCnpj(dto.CNPJ))
                throw new Exception("Já existe uma empresa cadastrada com este CNPJ.");

            var empresa = new Empresa () 
            {
               Nome = dto.Nome,
               CNPJ = dto.CNPJ
            };

            empresa.Slug = await GerarSlugUnico(dto.Nome);

            await _empresaRepository.Adicionar(empresa);

            return MapearParaResponseDto(empresa);
        }

        // Atualiza os dados cadastrais da empresa e regenera o slug único caso o nome seja alterado
        public async Task AtualizarEmpresa(long id, EmpresaUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var empresa = await _empresaRepository.ObterPorId(id);

            if (empresa == null)
                throw new Exception($"Empresa com id {id} não encontrada.");

            if (empresa.CNPJ != dto.CNPJ)
            {
                if (await _empresaRepository.ExisteCnpj(dto.CNPJ))
                    throw new Exception("O novo CNPJ informado já está em uso por outra empresa.");

                empresa.CNPJ = dto.CNPJ;
            }

            if (empresa.Nome != dto.Nome)
            {
                empresa.Nome = dto.Nome;
                empresa.Slug = await GerarSlugUnico(dto.Nome);
            }

            empresa.DataAtualizacao = DateTime.UtcNow;

            await _empresaRepository.Atualizar(empresa);
        }

        // Realiza a exclusão lógica da empresa e desativa o seu acesso ao sistema
        public async Task Deletar(long id)
        {
            var empresa = await _empresaRepository.ObterPorId(id);
            if (empresa == null)
                throw new Exception($"Empresa com id {id} não encontrada.");

            empresa.IsDeleted = true;
            empresa.Ativo = false;
            empresa.DataAtualizacao = DateTime.UtcNow;

            await _empresaRepository.Deletar(empresa);
        }

        #endregion

        #region Operações de Leitura (Consultas)

        // Busca uma empresa ativa através do seu Slug único para exibição pública ou administrativa
        public async Task<EmpresaResponseDto?> ObterEmpresaPorSlug(string slug)
        {
            return await _empresaRepository.ObterPorSlug(slug);
        }

        // Busca os detalhes de uma empresa através do seu ID técnico
        public async Task<EmpresaResponseDto?> ObterPorId(long id)
        {
            var empresa = await _empresaRepository.ObterPorId(id);

            if (empresa == null)
                throw new Exception($"Empresa com id {id} não encontrada.");

            return MapearParaResponseDto(empresa);
        }

        #endregion

        #region Métodos Privados e Mapeamentos

        // Gera uma URL amigável (slug) e garante sua unicidade adicionando sufixos numéricos se necessário
        private async Task<string> GerarSlugUnico(string nome)
        {
            var slugBase = StringHelper.ToSlug(nome);
            var slugFinal = slugBase;
            int contador = 1;

            while (await _empresaRepository.ExisteSlug(slugFinal))
            {
                slugFinal = $"{slugBase}-{contador++}";
            }

            return slugFinal;
        }

        // Converte a entidade de domínio Empresa para o DTO de resposta do sistema
        private static EmpresaResponseDto MapearParaResponseDto(Empresa empresa)
        {
            return new EmpresaResponseDto
            {
                Id = empresa.Id,
                Nome = empresa.Nome,
                CNPJ = empresa.CNPJ,
                Slug = empresa.Slug,
                DataCriacao = empresa.DataCriacao,
                DataAtualizacao = empresa.DataAtualizacao
            };
        }

        #endregion
    }
}