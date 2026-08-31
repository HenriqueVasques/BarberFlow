using BarberFlow.API.DTOs.Usuario;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces.IRepository;
using BarberFlow.API.Interfaces.IServices;
using BarberFlow.API.Models;
using BCryptLib = BCrypt.Net.BCrypt;

namespace BarberFlow.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
        }

        #region Comandos (Escrita)

        // Cadastra um novo usuário no sistema associado a uma empresa
        public async Task<UsuarioResponseDto> CriarUsuario(UsuarioCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Os dados não foram preenchidos.");

            var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId)
                ?? throw new KeyNotFoundException($"Empresa com ID {dto.EmpresaId} não encontrada.");

            if (await _usuarioRepository.ExisteEmail(dto.Email))
            {
                throw new InvalidOperationException("Este e-mail já está cadastrado.");
            }

            // Validação e sanitização (retorna null se vazio, lança exceção se inválido, ou retorna sanitizado)
            string? telefoneSanitizado = ValidarESanitizarTelefone(dto.Telefone, "Telefone");
            string? whatsappSanitizado = ValidarESanitizarTelefone(dto.Whatsapp, "WhatsApp");

            string senhaHash = CriptografarSenha(dto.Senha);

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = telefoneSanitizado,
                Whatsapp = whatsappSanitizado,
                SenhaHash = senhaHash,
                EmpresaId = dto.EmpresaId,
                Perfil = PerfilUsuario.Administrador,
                Ativo = true,
                IsDeleted = false,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _usuarioRepository.Adicionar(usuario);
            return MapToResponseDto(usuario);
        }

        // Atualiza os dados cadastrais do usuário (nome, e-mail, telefone, whatsapp)
        public async Task AtualizarUsuario(long id, UsuarioUpdateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Os dados não foram preenchidos.");

            var usuario = await _usuarioRepository.ObterPorId(id)
                ?? throw new KeyNotFoundException($"Usuário com ID {id} não encontrado.");

            if (!string.Equals(usuario.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (await _usuarioRepository.ExisteEmail(dto.Email))
                    throw new InvalidOperationException("O novo e-mail informado já está em uso por outro usuário.");

                usuario.Email = dto.Email;
            }

            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new InvalidOperationException("O nome precisa ser preenchido.");

            usuario.Nome = dto.Nome;
            usuario.Telefone = ValidarESanitizarTelefone(dto.Telefone, "Telefone");
            usuario.Whatsapp = ValidarESanitizarTelefone(dto.Whatsapp, "WhatsApp");
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _usuarioRepository.Atualizar(usuario);
        }

        // Executa o Soft Delete e desativa o usuário
        public async Task DeletarUsuario(long id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id)
                ?? throw new KeyNotFoundException($"Usuário com ID {id} não encontrado.");

            usuario.IsDeleted = true;
            usuario.Ativo = false;
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _usuarioRepository.Deletar(usuario);
        }

        // Altera e criptografa a nova senha do usuário
        public async Task AlterarSenha(long id, UsuarioAlterarSenhaDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Senha))
            {
                throw new ArgumentNullException(nameof(dto), "A nova senha é obrigatória.");
            }

            var usuario = await _usuarioRepository.ObterPorId(id)
                ?? throw new KeyNotFoundException($"Usuário com ID {id} não encontrado.");

            usuario.SenhaHash = CriptografarSenha(dto.Senha);
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _usuarioRepository.AlterarSenha(usuario);
        }

        #endregion

        #region Consultas (Leitura)

        // Busca um usuário específico por ID e retorna formatado no DTO de resposta
        public async Task<UsuarioResponseDto> ObterPorId(long id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id)
                ?? throw new KeyNotFoundException($"Usuário com ID {id} não encontrado.");

            return MapToResponseDto(usuario);
        }

        // Lista todos os usuários cadastrados vinculados a uma determinada empresa
        public async Task<IEnumerable<UsuarioResponseDto>> ObterUsuariosPorEmpresa(long empresaId)
        {
            var empresa = await _empresaRepository.ObterPorId(empresaId)
                ?? throw new KeyNotFoundException($"Empresa com ID {empresaId} não encontrada.");

            return await _usuarioRepository.ObterPorEmpresa(empresaId);
        }

        #endregion

        #region Métodos Auxiliares Privados

        // Sanitiza a string (remove caracteres não numéricos) e valida o formato do número
        private static string? ValidarESanitizarTelefone(string? numero, string nomeCampo)
        {
            if (string.IsNullOrWhiteSpace(numero))
                return null;

            // Remove tudo que não for dígito
            string apenasDigitos = new string(numero.Where(char.IsDigit).ToArray());

            // Padrão brasileiro: DDD (2 dígitos) + 8 dígitos (fixo) ou 9 dígitos (celular) = 10 a 11 dígitos
            if (apenasDigitos.Length < 10 || apenasDigitos.Length > 11)
            {
                throw new InvalidOperationException($"O campo {nomeCampo} deve conter um número válido com DDD (10 ou 11 dígitos).");
            }

            return apenasDigitos;
        }

        // Gera o hash seguro da senha utilizando BCrypt
        private static string CriptografarSenha(string senha)
        {
            return BCryptLib.HashPassword(senha);
        }

        // Mapeia a entidade de domínio Usuario para o DTO de resposta da API
        private static UsuarioResponseDto MapToResponseDto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                EmpresaId = usuario.EmpresaId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Telefone = usuario.Telefone,
                Whatsapp = usuario.Whatsapp,
                DataCriacao = usuario.DataCriacao,
                DataAtualizacao = usuario.DataAtualizacao,
                IsDeleted = usuario.IsDeleted,
                Ativo = usuario.Ativo
            };
        }

        #endregion
    }
}