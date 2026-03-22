using BarberFlow.API.DTOs.Empresa;
using BarberFlow.API.Helpers;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Services
{
    public class EmpresaService
    {
        #region Readonly Fields
        private readonly IEmpresaRepository _repository;
        #endregion

        #region Constructor
        public EmpresaService(IEmpresaRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Private Methods
        private async Task<string> GerarSlugUnico(string nome)
        {
            var slugBase = StringHelper.ToSlug(nome);
            var slugFinal = slugBase;
            int contador = 1;

            while (await _repository.ExisteSlug(slugFinal))
            {
                slugFinal = $"{slugBase}-{contador++}";
            }

            return slugFinal;
        }
        #endregion

        #region Public Methods
        public async Task<Empresa> CriarEmpresa(EmpresaCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os ados não foram preenchidos.");

            if (await _repository.ExisteCnpj(dto.CNPJ))
                throw new Exception("Já existe uma empresa cadastrada com este CNPJ.");

            var empresa = new Empresa(dto.Nome, dto.CNPJ);
            empresa.Slug = await GerarSlugUnico(dto.Nome);

            await _repository.Adicionar(empresa);
            return empresa;
        }

        public async Task<Empresa?> AtualizarEmpresa(long id, EmpresaUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os ados não foram preenchidos.");

            var empresa = await _repository.ObterPorId(id);

            if (empresa == null) 
                throw new Exception($"Empresa com id {id} não encontrada.");


            if (empresa.CNPJ != dto.CNPJ)
            {
                if (await _repository.ExisteCnpj(dto.CNPJ))
                    throw new Exception("O novo CNPJ informado já está em uso por outra empresa.");

                empresa.CNPJ = dto.CNPJ;
            }

            if (empresa.Nome != dto.Nome)
            {
                empresa.Nome = dto.Nome;
                empresa.Slug = await GerarSlugUnico(dto.Nome);
            }

            empresa.DataAtualizacao = DateTime.UtcNow;

            await _repository.Atualizar(empresa);
            return empresa;
        }

        public async Task <Empresa?> Deletar(long id)
        {
            var empresa = await _repository.ObterPorId(id);
            if ( empresa == null)
                throw new Exception($"Empresa com id {id} não encontrada.");

            empresa.IsDeleted = true;
            empresa.Ativo = false;

            await _repository.Deletar(empresa);
            return empresa;

        }

        public async Task<Empresa?> ObterEmpresaPorSlug(string slug)
        {
            return await _repository.ObterPorSlug(slug);
        }
        #endregion
    }
}