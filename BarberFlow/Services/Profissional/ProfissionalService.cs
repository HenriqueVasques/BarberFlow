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
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly AppDbContext _appDbContext;

        public ProfissionalService(IProfissionalRepository profissionalRepository,IUsuarioRepository usuarioRepository,IEmpresaRepository empresaRepository, IAgendamentoRepository agendamentoRepository , AppDbContext appDbContext)
        {
            _profissionalRepository = profissionalRepository;
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _agendamentoRepository = agendamentoRepository;
            _appDbContext = appDbContext;
        }

        public async Task<Profissional?> CriarProfissional(ProfissionalCreateDto dto)
        {
            using IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                if (dto == null)
                    throw new Exception("Os ados não foram preenchidos.");

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
                    dto.Telefone,
                    dto.Whatsapp,
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

        public async Task<Profissional> AtualizarProfissional(long id, ProfissionalUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os ados não foram preenchidos.");

            var profissional = await _profissionalRepository.ObterPorId(id);
            if (profissional == null)
                throw new Exception($"Profissional com id {id} não encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Email) && profissional.Usuario.Email != dto.Email)
            {
                if (await _usuarioRepository.ExisteEmail(dto.Email))
                {
                    throw new Exception("Este e-mail já está em uso por outro usuário.");
                }
                profissional.Usuario.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.Nome)) profissional.Usuario.Nome = dto.Nome;
            if (!string.IsNullOrWhiteSpace(dto.Telefone)) profissional.Usuario.Telefone = dto.Telefone;
            if (!string.IsNullOrWhiteSpace(dto.Whatsapp)) profissional.Usuario.Whatsapp = dto.Whatsapp;
            profissional.Usuario.DataAtualizacao = DateTime.UtcNow;
            profissional.PercentualComissao = dto.PercentualComissao;
            profissional.DataAtualizacao = DateTime.UtcNow;

            await _profissionalRepository.Atualizar(profissional);
            var profissionalRecuperado = await _profissionalRepository.ObterPorId(profissional.Id);
            return profissionalRecuperado ?? throw new Exception("Erro ao recuperar Cliente criado.");
        }

        public async Task<Profissional?> DeletarProfissional(long id)
        {

            var profissional = await _profissionalRepository.ObterPorId(id);
            if (profissional == null)
                throw new Exception($"Profissional com id {id} não encontrado.");

            var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
            var futuroProximo = hoje.AddYears(2);

            var statusAtivos = new List<StatusAgendamento>
            {
                StatusAgendamento.Pendente,
                StatusAgendamento.Confirmado
            };

            var agendamentosFuturos = await _agendamentoRepository.ObterAgendaPorPeriodo(profissional.Id, profissional.EmpresaId, hoje, futuroProximo, statusAtivos);

            if (agendamentosFuturos != null && agendamentosFuturos.Any())
            {
                throw new Exception($"Não é possível deletar o profissional {profissional.Usuario.Nome}. " +
                                    $"Existem {agendamentosFuturos.Count} agendamentos pendentes ou confirmados na agenda dele");
            }

            profissional.IsDeleted = true;
            profissional.Ativo = false;
            profissional.DataAtualizacao = DateTime.UtcNow;

            if (profissional.Usuario != null)
            {
                profissional.Usuario.Telefone = "000000000";
                profissional.Usuario.Whatsapp = "000000000";
                profissional.Usuario.Nome = "Usuário Excluído";
                profissional.Usuario.Email = $"excluido_{profissional.Id}@barberflow.com.br";
                profissional.Usuario.SenhaHash = "";
                profissional.Usuario.DataAtualizacao = DateTime.UtcNow;
            }

            await _profissionalRepository.Deletar(profissional);

            return profissional;
        }

        public async Task<IEnumerable<Profissional>> ObterProfissionaisPorEmpresa(long empresaId)
        {
            if (await _empresaRepository.ObterPorId(empresaId) == null)
                throw new Exception($"Empresa com id {empresaId} não encontrada.");
  
            var profissionais =  await _profissionalRepository.ObterPorEmpresa(empresaId);
            if(profissionais == null || !profissionais.Any()) throw new Exception($"Empresa com ID {empresaId} não tem profissionais cadastrados.");
            return profissionais;
        }

        public async Task<Profissional> ObterPorId(long id)
        {
            var profissional = await _profissionalRepository.ObterPorId(id);
            if (profissional == null)
                throw new Exception($"Profissional com id {id} não encontrado.");

            return profissional;
        }
    }
}