using BarberFlow.API.DTOs.Usuario;

namespace BarberFlow.API.Interfaces.IServices
{
    public interface IUsuarioService
    {
        #region Comandos (Escrita)

        Task<UsuarioResponseDto> CriarUsuario(UsuarioCreateDto dto);
        Task AtualizarUsuario(long id, UsuarioUpdateDto dto);
        Task DeletarUsuario(long id);
        Task AlterarSenha(long id, UsuarioAlterarSenhaDto dto);

        #endregion

        #region Consultas (Leitura)

        Task<UsuarioResponseDto> ObterPorId(long id);
        Task<IEnumerable<UsuarioResponseDto>> ObterUsuariosPorEmpresa(long empresaId);

        #endregion
    }
}