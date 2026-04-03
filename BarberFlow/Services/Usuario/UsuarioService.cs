using BarberFlow.API.Data.Repositories;
using BarberFlow.API.DTOs.Usuario;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using BCryptLib = BCrypt.Net.BCrypt;
namespace BarberFlow.API.Services
{
    public class UsuarioService
    {
        #region private fields
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;
        #endregion
        #region private methods
        private string CriptografarSenha(string senha)
        {
            return BCryptLib.HashPassword(senha);
        }
        #endregion

        #region constructors
        public UsuarioService(IUsuarioRepository repository, IEmpresaRepository empresaRepository)
        {
            _usuarioRepository = repository;
            _empresaRepository = empresaRepository;
        }
        #endregion

        #region public methods
        public async Task<Usuario?> CriarUsuario(UsuarioCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os ados não foram preenchidos.");

            var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId);
            if (empresa == null)
            {
                throw new Exception("A empresa não existe.");
            }

            if (await _usuarioRepository.ExisteEmail(dto.Email))
            {
                throw new Exception("Este e-mail já está cadastrado.");
            }

            string senhaHash = CriptografarSenha(dto.Senha);

            var usuario = new Usuario 
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Whatsapp = dto.Whatsapp,
                SenhaHash = senhaHash,
                EmpresaId = dto.EmpresaId,
                Perfil = PerfilUsuario.Administrador,
                Ativo = true,
                IsDeleted = false,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow

            };
               
            await _usuarioRepository.Adicionar(usuario);
            return usuario;
        }

        public async Task<Usuario?> AtualizarUsuario(long id, UsuarioUpdateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "O objeto dto não pode ser nulo.");

            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null)
                throw new Exception($"Usuário com id {id} não encontrada.");

            if (usuario.Email != dto.Email)
            {
                if (await _usuarioRepository.ExisteEmail(dto.Email))
                    throw new Exception("O novo e-mail informado já está em uso por outro usuário.");

                usuario.Email = dto.Email;
            }
            if (dto.Nome == null)

                throw new Exception("O nome precisa ser preenchido.");

            usuario.Nome = dto.Nome;
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _usuarioRepository.Atualizar(usuario);
            return usuario;
        }

        public async Task<Usuario?> DeletarUsuario(long id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null)
                throw new Exception($"Usuário com id {id} não encontrada.");

            usuario.IsDeleted = true;
            usuario.Ativo = false;

            await _usuarioRepository.Deletar(usuario);
            return usuario;
        }

        public async Task AlterarSenha(long id, UsuarioAlterarSenhaDto dto)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null)
                throw new Exception($"Usuário com id {id} não encontrada.");

            string senhaHash = CriptografarSenha(dto.Senha);
            usuario.SenhaHash = senhaHash;
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _usuarioRepository.AlterarSenha(usuario);
        }

        public async Task<Usuario?> ObterUsuarioPorId(long id)
        {
            return await _usuarioRepository.ObterPorId(id) ?? throw new Exception("A empresa não existe.");
        }

        public async Task<IEnumerable<Usuario>> ObterUsuariosPorEmpresa(long empresaId)
        {
            if (await _empresaRepository.ObterPorId(empresaId) == null)
                throw new Exception("A empresa não existe.");

            return await _usuarioRepository.ObterPorEmpresa(empresaId);
        }
        #endregion
    }
}
