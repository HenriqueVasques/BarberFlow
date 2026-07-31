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

        public ProfissionalService(
            IProfissionalRepository profissionalRepository,
            IUsuarioRepository usuarioRepository,
            IEmpresaRepository empresaRepository,
            IAgendamentoRepository agendamentoRepository,
            AppDbContext appDbContext)
        {
            _profissionalRepository = profissionalRepository;
            _usuarioRepository = usuarioRepository;
            _empresaRepository = empresaRepository;
            _agendamentoRepository = agendamentoRepository;
            _appDbContext = appDbContext;
        }

        #region Comandos: Escrita (Admin / Gestão)

        // Cadastra um novo profissional criando a conta de Usuário e o vínculo com a Empresa em uma única transação
        public async Task<ProfissionalResponseDto> CriarProfissional(ProfissionalCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            if (await _usuarioRepository.ExisteEmail(dto.Email))
                throw new Exception("Este e-mail já está em uso.");

            var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId)
                ?? throw new Exception($"Empresa com ID {dto.EmpresaId} não encontrada.");

            using IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
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

                return MapToResponseDto(profissional, usuario.Nome, empresa.Nome, usuario.Email);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Atualiza os dados cadastrais do perfil do profissional e do usuário vinculado
        public async Task AtualizarProfissional(long id, ProfissionalUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Os dados não foram preenchidos.");

            var profissional = await _profissionalRepository.ObterPorId(id)
                ?? throw new Exception($"Profissional com ID {id} não encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Email) && profissional.Usuario.Email != dto.Email)
            {
                if (await _usuarioRepository.ExisteEmail(dto.Email))
                    throw new Exception("Este e-mail já está em uso por outro usuário.");

                profissional.Usuario.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.Nome))
                profissional.Usuario.Nome = dto.Nome;

            if (!string.IsNullOrWhiteSpace(dto.Telefone))
                profissional.Usuario.Telefone = dto.Telefone;

            if (!string.IsNullOrWhiteSpace(dto.Whatsapp))
                profissional.Usuario.Whatsapp = dto.Whatsapp;

            profissional.Usuario.DataAtualizacao = DateTime.UtcNow;
            profissional.PercentualComissao = dto.PercentualComissao;
            profissional.DataAtualizacao = DateTime.UtcNow;

            await _profissionalRepository.Atualizar(profissional);
        }

        // Realiza o Soft Delete do profissional e anonimiza os dados sensíveis do Usuário associado
        public async Task DeletarProfissional(long id)
        {
            var profissional = await _profissionalRepository.ObterPorId(id)
                ?? throw new Exception($"Profissional com ID {id} não encontrado.");

            var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
            var futuroProximo = hoje.AddYears(2);

            var statusAtivos = new List<StatusAgendamento>
            {
                StatusAgendamento.Pendente,
                StatusAgendamento.Confirmado
            };

            var agendamentosFuturos = await _agendamentoRepository.ObterAgendaPorPeriodo(
                profissional.Id,
                profissional.EmpresaId,
                hoje,
                futuroProximo,
                statusAtivos
            );

            if (agendamentosFuturos != null && agendamentosFuturos.Any())
            {
                throw new Exception($"Não é possível deletar o profissional {profissional.Usuario.Nome}. " +
                                    $"Existem {agendamentosFuturos.Count} agendamento(s) pendente(s) ou confirmado(s) na agenda dele.");
            }

            // Desativa o registro do Profissional
            profissional.IsDeleted = true;
            profissional.Ativo = false;
            profissional.DataAtualizacao = DateTime.UtcNow;

            // Anonimização LGPD para a conta de usuário associada
            if (profissional.Usuario != null)
            {
                profissional.Usuario.Telefone = "000000000";
                profissional.Usuario.Whatsapp = "000000000";
                profissional.Usuario.Nome = "Usuário Excluído";
                profissional.Usuario.Email = $"excluido_{profissional.Id}@barberflow.com.br";
                profissional.Usuario.SenhaHash = string.Empty;
                profissional.Usuario.DataAtualizacao = DateTime.UtcNow;
            }

            await _profissionalRepository.Deletar(profissional);
        }

        #endregion

        #region Consultas: Leitura

        // Obtém o profissional com detalhes mapeados pelo seu ID
        public async Task<ProfissionalResponseDto> ObterPorId(long id)
        {
            var profissional = await _profissionalRepository.ObterPorId(id)
                ?? throw new Exception($"Profissional com ID {id} não encontrado.");

            return MapToResponseDto(profissional);
        }

        // Lista todos os profissionais vinculados a uma determinada Empresa
        public async Task<IEnumerable<ProfissionalResponseDto>> ObterProfissionaisPorEmpresa(long empresaId)
        {
            var empresa = await _empresaRepository.ObterPorId(empresaId)
                ?? throw new Exception($"Empresa com ID {empresaId} não encontrada.");

            var profissionais = await _profissionalRepository.ObterPorEmpresa(empresaId);

            if (profissionais == null || !profissionais.Any())
                throw new Exception($"Empresa com ID {empresaId} não possui profissionais cadastrados.");

            return profissionais;
        }

        #endregion

        #region Métodos Auxiliares Privados

        private ProfissionalResponseDto MapToResponseDto(Profissional profissional, string? nome = null, string? nomeEmpresa = null, string? email = null)
        {
            return new ProfissionalResponseDto
            {
                Id = profissional.Id,
                EmpresaId = profissional.EmpresaId,
                UsuarioId = profissional.UsuarioId,
                Nome = nome ?? profissional.Usuario?.Nome,
                NomeEmpresa = nomeEmpresa ?? profissional.Empresa?.Nome,
                Email = email ?? profissional.Usuario?.Email,
                PercentualComissao = profissional.PercentualComissao,
                DataCriacao = profissional.DataCriacao,
                DataAtualizacao = profissional.DataAtualizacao,
                Ativo = profissional.Ativo
            };
        }

        #endregion
    }
}