using BarberFlow.API.DTOs.Auth;
using BarberFlow.API.DTOs.Usuario;
using BarberFlow.API.Models;

namespace BarberFlow.API.Interfaces
{
    public interface IUsuarioRepository
    {
        #region Persistência e Comandos (Escrita)

        Task Adicionar(Usuario usuario);
        Task Atualizar(Usuario usuario);
        Task Deletar(Usuario usuario);
        Task AlterarSenha(Usuario usuario);

        #endregion

        #region Consultas (Leitura)

        Task<bool> ExisteEmail(string email);
        Task<Usuario?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<List<UsuarioResponseDto>> ObterPorEmpresa(long empresaId, bool apenasAtivos = true, bool incluirDeletados = false);
        Task<Usuario?> ObterPorEmail(string email);

        #endregion
    }
}