using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Profissional;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace BarberFlow.API.Services
{
    public class ProfissionalService
    {
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly AppDbContext _appDbContext;

        public ProfissionalService(IProfissionalRepository profissionalRepository,IUsuarioRepository usuarioRepository,IEmpresaRepository empresaRepository,AppDbContext appDbContext)
        {
            _profissionalRepository = profissionalRepository;
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _appDbContext = appDbContext;
        }

        public async Task<Profissional?> CriarProfissional(ProfissionalCreateDto dto)
        {
            using IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {
                if (await _usuarioRepository.ExisteEmail(dto.Email))
                {
                    throw new Exception("Este e-mail já está em uso.");
                }
                    

                var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId);
                if (empresa == null)
                {
                    throw new Exception("Empresa não encontrada.");
                }

                string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

                var usuario = new Usuario(
                    dto.Nome,
                    dto.Email,
                    senhaHash,
                    dto.EmpresaId,
                    PerfilUsuario.Profissional
                );

                await _usuarioRepository.Adicionar(usuario);

                var profissional = new Profissional(
                    dto.EmpresaId,
                    usuario.Id,
                    dto.PercentualComissao
                );

                await _profissionalRepository.Adicionar(profissional);

                await transaction.CommitAsync();

                return profissional;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Profissional?> AtualizarProfissional(long id, ProfissionalUpdateDto dto)
        {
            var profissional = await _profissionalRepository.ObterPorId(id);
            if (profissional == null)
            {
                throw new Exception($"Profissional com id {id} não encontrado.");
            }

            if(profissional.Usuario.Email != dto.Email)
            {
                if (await _usuarioRepository.ExisteEmail(dto.Email))
                {
                    throw new Exception("Este e-mail já está em uso.");
                }
            }

            profissional.Usuario.Nome = dto.Nome;
            profissional.Usuario.Email = dto.Email;
            profissional.Usuario.DataAtualizacao = DateTime.UtcNow;
            profissional.PercentualComissao = dto.PercentualComissao;
            profissional.DataAtualizacao = DateTime.UtcNow;

            await _profissionalRepository.Atualizar(profissional);
            return profissional;
        }

        public async Task<Profissional?> DeletarProfissional(long id)
        {
            var profissional = await _profissionalRepository.ObterPorId(id);
            if (profissional == null)
            {
                throw new Exception($"Profissional com id {id} não encontrado.");
            }
            profissional.IsDeleted = true;
            profissional.Ativo = false;
            profissional.DataAtualizacao = DateTime.UtcNow;

            profissional.Usuario.IsDeleted = true;
            profissional.Usuario.Ativo = false;
            profissional.Usuario.DataAtualizacao = DateTime.UtcNow;

            await _profissionalRepository.Deletar(profissional);

            return profissional;
        }
    }
}
