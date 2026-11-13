using BarberFlow.API.DTOs.Auth;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces.IRepository;
using BarberFlow.API.Interfaces.IServices;
using BarberFlow.API.Models;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberFlow.API.Services
{
    public class AuthService : IAuthService
    {
        #region Atributos e Construtor

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmpresaRepository _empresaRepository;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration, IEmpresaRepository empresaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _empresaRepository = empresaRepository;
        }

        #endregion

        #region Regras de Negócio (Autenticação)

        // Realiza a validação de e-mail e senha e retorna o token JWT e dados do usuário
        public async Task<LoginResponseDto> Login(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Senha))
                throw new InvalidOperationException("E-mail ou senha inválidos.");

            var usuario = await _usuarioRepository.ObterPorEmail(dto.Email)
                ?? throw new Exception("E-mail ou senha inválidos.");

            if (string.IsNullOrWhiteSpace(usuario.SenhaHash))
                throw new InvalidOperationException("Esta conta foi criada via Google. Por favor, utilize o login do Google.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                throw new Exception("E-mail ou senha inválidos.");

            // Calcula o tempo de expiração e gera o Token JWT
            var expiracaoHoras = double.Parse(_configuration["JwtSettings:ExpiracaoHoras"] ?? "8");
            var expiracao = DateTime.UtcNow.AddHours(expiracaoHoras);
            var token = GerarToken(usuario, expiracao);

            return new LoginResponseDto
            {
                Token = token,
                Expiracao = expiracao,
                Id = usuario.Id,
                EmpresaId = usuario.EmpresaId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil
            };
        }

        public async Task<string> LoginComGoogle(GoogleLoginDto dto)
        {
            if(dto == null || string.IsNullOrEmpty(dto.IdToken))
                throw new ArgumentException("Token do Google não fornecido.");
      
            GoogleJsonWebSignature.Payload payload;

            try
            {
                var settigs = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { _configuration["Google:ClientId"] }
                    ?? throw new InvalidOperationException("O ClientId do Google não foi configurado.") 
                };

                payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, settigs);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Token do Google inválido ou expirado.");
            }

            var usuario = await _usuarioRepository.ObterPorEmail(payload.Email);

            if (usuario == null)
            {
                if (!dto.EmpresaId.HasValue || dto.EmpresaId.Value <= 0)
                    throw new InvalidOperationException("Empresa não informada.");

                var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId.Value);

                if (empresa == null)
                    throw new InvalidOperationException("Empresa não encontrada.");

                usuario = new Usuario
                {
                    EmpresaId = empresa.Id,
                    Nome = payload.Name ?? payload.Email,
                    Email = payload.Email,
                    SenhaHash = string.Empty,
                    Perfil = PerfilUsuario.Cliente,
                    Ativo = true,
                    IsDeleted = false,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                };
                await _usuarioRepository.Adicionar(usuario);
            }
            // Calcula o tempo de expiração e gera o Token JWT
            var expiracaoHoras = double.Parse(_configuration["JwtSettings:ExpiracaoHoras"] ?? "8");
            var expiracao = DateTime.UtcNow.AddHours(expiracaoHoras);
            var token = GerarToken(usuario, expiracao);
            return token;
        }


        #endregion

        #region Métodos Privados (Auxiliares JWT)

        // Constrói e assina o Token JWT contendo as informações (Claims) do usuário logado
        private string GerarToken(Usuario usuario, DateTime expiracao)
        {
            var secretKey = _configuration["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("A chave secreta JWT não foi configurada.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("nome", usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Perfil.ToString()),
                new Claim("empresaId", usuario.EmpresaId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expiracao,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}