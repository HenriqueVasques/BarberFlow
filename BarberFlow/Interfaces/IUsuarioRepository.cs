using BarberFlow.API.DTOs.Usuario;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task Adicionar(Usuario usuario);
        public Task<Usuario?> ObterPorId(long id);
        public Task Atualizar(Usuario usuario);
        public Task Deletar(Usuario usuario);
        public Task<bool> ExisteEmail(string email);
        public Task AlterarSenha(Usuario usuario);
    }
}
