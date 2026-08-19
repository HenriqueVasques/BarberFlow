using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Cliente;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace BarberFlow.API.Services
{
    public class ClienteService
    {
        #region Readonly Fields
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly AppDbContext _appDbContext;
        #endregion

        #region Construtor
        public ClienteService(IClienteRepository clienteRepository, IEmpresaRepository empresaRepository, IUsuarioRepository usuarioRepository, AppDbContext appDbContext)
        {
            _clienteRepository = clienteRepository;
            _empresaRepository = empresaRepository;
            _usuarioRepository = usuarioRepository;
            _appDbContext = appDbContext;
        }
        #endregion

        #region Ações de Escrita (Regras de Negócio)

        // Orquestra a criação do cliente e usuário vinculado dentro de uma transação única
        public async Task<ClienteResponseDto> CriarCliente(ClienteCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "O objeto dto não pode ser nulo.");

            if (!dto.EmpresaId.HasValue)
            {
                throw new InvalidOperationException("A empresa é obrigatória.");
            }

            long empresaId = dto.EmpresaId.Value;

            using IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                var empresa = await _empresaRepository.ObterPorId(empresaId);
                if (empresa == null)
                    throw new KeyNotFoundException($"Empresa com ID {empresaId} não encontrada.");

                if (await _usuarioRepository.ExisteEmail(dto.Email))
                    throw new InvalidOperationException($"O email {dto.Email} já está em uso.");

                string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

                var usuario = new Usuario
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Telefone = dto.Telefone,
                    Whatsapp = dto.Whatsapp,
                    SenhaHash = senhaHash,
                    EmpresaId = empresaId,
                    Perfil = PerfilUsuario.Cliente,
                };

                await _usuarioRepository.Adicionar(usuario);

                var cliente = new Cliente
                {
                    EmpresaId = empresaId,
                    UsuarioId = usuario.Id,
                };

                await _clienteRepository.Adicionar(cliente);
                await transaction.CommitAsync();

                return new ClienteResponseDto
                {
                    Id = cliente.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Telefone = usuario.Telefone,
                    Whatsapp = usuario.Whatsapp,
                    EmpresaId = cliente.EmpresaId,
                    UsuarioId = cliente.UsuarioId,
                    DataCriacao = cliente.DataCriacao,
                    DataAtualizacao = cliente.DataAtualizacao,
                    Ativo = cliente.Ativo,
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Atualiza os dados de contato do usuário vinculado ao cliente
        public async Task AtualizarCliente(long id, ClienteUpdateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "O objeto dto não pode ser nulo.");

            var cliente = await _clienteRepository.ObterClienteComUsuario(id)
                ?? throw new KeyNotFoundException($"Cliente com ID {id} não encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Nome))
                cliente.Usuario.Nome = dto.Nome;

            if (!string.IsNullOrWhiteSpace(dto.Telefone))
                cliente.Usuario.Telefone = dto.Telefone;

            if (!string.IsNullOrWhiteSpace(dto.Whatsapp))
                cliente.Usuario.Whatsapp = dto.Whatsapp;

            cliente.DataAtualizacao = DateTime.UtcNow;

            await _clienteRepository.Atualizar(cliente);
        }

        // Realiza a exclusão lógica do cliente e anonimiza os dados do usuário (LGPD)
        public async Task DeletarCliente(long id)
        {
            var cliente = await _clienteRepository.ObterClienteComUsuario(id)
                ?? throw new KeyNotFoundException($"Cliente com ID {id} não encontrado.");

            cliente.IsDeleted = true;
            cliente.Ativo = false;
            cliente.DataAtualizacao = DateTime.UtcNow;

            if (cliente.Usuario == null)
                throw new InvalidOperationException($"Cliente com ID {id} não tem usuário associado.");

            cliente.Usuario.Telefone = "000000000";
            cliente.Usuario.Whatsapp = "000000000";
            cliente.Usuario.Nome = "Usuário Excluído";
            cliente.Usuario.Email = $"excluido_{cliente.Id}@barberflow.com.br";
            cliente.Usuario.SenhaHash = "";
            cliente.Usuario.DataAtualizacao = DateTime.UtcNow;

            await _clienteRepository.Deletar(cliente);
            await _usuarioRepository.Atualizar(cliente.Usuario);
        }

        #endregion

        #region Consultas (Leitura)

        // Busca os dados básicos de um cliente específico
        public async Task<ClienteResponseDto> ObterClientePorId(long id, bool incluirDeletados)
        {
            var cliente = await _clienteRepository.ObterPorId(id, incluirDeletados)
                ?? throw new KeyNotFoundException($"Cliente com ID {id} não encontrado.");

            return MapearParaResponseDto(cliente);
        }

        // Recupera a listagem de clientes de uma empresa com suporte a paginação
        public async Task<IEnumerable<ClienteResponseDto>> ObterClientesPorEmpresa(long empresaId, bool incluirDeletados, int pagina)
        {
            var empresa = await _empresaRepository.ObterPorId(empresaId);
            if (empresa == null)
                throw new KeyNotFoundException($"Empresa com ID {empresaId} não encontrada.");

            var clientes = await _clienteRepository.ObterPorEmpresa(empresaId, incluirDeletados, pagina);

            return clientes ?? Enumerable.Empty<ClienteResponseDto>();
        }

        #endregion

        #region Mapeamentos Privados

        // Converte a entidade Cliente para o DTO de resposta (campos básicos)
        private static ClienteResponseDto MapearParaResponseDto(Cliente cliente)
        {
            return new ClienteResponseDto
            {
                Id = cliente.Id,
                EmpresaId = cliente.EmpresaId,
                UsuarioId = cliente.UsuarioId,
                DataCriacao = cliente.DataCriacao,
                DataAtualizacao = cliente.DataAtualizacao,
                Ativo = cliente.Ativo
            };
        }

        #endregion
    }
}