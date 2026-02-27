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

        #region Public Methods
        public async Task<Empresa> CriarEmpresa(EmpresaCreateDto dto)
        {
            var empresa = new Empresa(dto.Nome, dto.CNPJ);
            empresa.Slug = await GerarSlugUnico(dto.Nome);

            await _repository.Adicionar(empresa);
            return empresa;
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
    }
}