    using BarberFlow.API.Models;

    namespace BarberFlow.API.Interfaces
    {
        public interface IEmpresaRepository
        {
            Task Adicionar(Empresa empresa);
            Task<bool> ExisteSlug(string slug);
            Task<Empresa?> ObterPorId(long id);
        }
    }