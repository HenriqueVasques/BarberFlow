using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BarberFlow.API.DTOs.Auth;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace BarberFlow.API.Services
{
    public class AuthService
    {
        #region Atributos e Construtor

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        #endregion

        #region Regras de Negócio (Autenticação)

        // Realiza a validação de e-mail e senha e retorna o token JWT e dados do usuário
        public async Task<LoginResponseDto> Login(LoginDto dto)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(dto.Email)
                ?? throw new Exception("E-mail ou senha inválidos.");

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
                new Claim("perfil", usuario.Perfil.ToString()),
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