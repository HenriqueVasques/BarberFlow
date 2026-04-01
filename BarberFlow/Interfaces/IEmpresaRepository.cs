    using BarberFlow.API.Models;

    namespace BarberFlow.API.Interfaces
    {
        public interface IEmpresaRepository
        {
            Task Adicionar(Empresa empresa);
            Task Atualizar(Empresa empresa);
            Task Deletar(Empresa empresa);
            Task<bool> ExisteSlug(string slug);
            Task<Empresa?> ObterPorId(long id);
            Task<bool> ExisteCnpj(string cnpj);
            Task<Empresa?> ObterPorSlug(string slug);              
        }
    }