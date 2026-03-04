using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;

namespace BarberFlow.API.Data.Repositories
{
    public class ProfissionalRepository : IProfissionalRepository
    {
        public Task Adicionar(Profissional profissional)
        {
            throw new NotImplementedException();
        }

        public Task Atualizar(Profissional profissional)
        {
            throw new NotImplementedException();
        }

        public Task Deletar(Profissional profissional)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Profissional>> ObterPorEmpresa(long empresaId)
        {
            throw new NotImplementedException();
        }

        public Task<Profissional?> ObterPorId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
