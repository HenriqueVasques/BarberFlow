using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Cliente;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace BarberFlow.API.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly AppDbContext _appDbContext;

        public ClienteService(IClienteRepository clienteRepository, IEmpresaRepository empresaRepository, IUsuarioRepository usuarioRepository, AppDbContext appDbContext)
        {
            _clienteRepository = clienteRepository;
            _empresaRepository = empresaRepository;
            _usuarioRepository = usuarioRepository;
            _appDbContext = appDbContext;
        }

        public async Task<Cliente> CriarCliente(ClienteCreateDto dto)
        {
            using IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "O objeto dto não pode ser nulo.");

                var empresa = await _empresaRepository.ObterPorId(dto.EmpresaId);
                if (empresa == null) throw new Exception($"Empresa com ID {dto.EmpresaId} não encontrada.");

                if(await _usuarioRepository.ExisteEmail(dto.Email)) throw new Exception($"O email {dto.Email} já está em uso.");

                string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

                var usuario = new Usuario(
                    dto.Nome,
                    dto.Email,
                    senhaHash,
                    dto.EmpresaId,
                    PerfilUsuario.Cliente
                );

                await _usuarioRepository.Adicionar(usuario);

                var cliente = new Cliente(
                    dto.EmpresaId,
                    usuario.Id,
                    dto.Telefone,
                    dto.Whatsapp
                );

                await _clienteRepository.Adicionar(cliente);
                await transaction.CommitAsync();
                return cliente;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var erroReal = ex.InnerException?.Message;
                throw;
            }
        }

        public async Task<Cliente> AtualizarCliente(long id, ClienteUpdateDto dto) 
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "O objeto dto não pode ser nulo.");

            var cliente = await _clienteRepository.ObterPorId(id) 
                ?? throw new Exception($"Cliente com ID {id} não encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Nome))
               cliente.Usuario.Nome = dto.Nome;

            if (!string.IsNullOrWhiteSpace(dto.Telefone))
                cliente.Telefone = dto.Telefone;

            if (!string.IsNullOrWhiteSpace(dto.Whatsapp))
                cliente.Whatsapp = dto.Whatsapp;

            cliente.DataAtualizacao = DateTime.UtcNow;

            await _clienteRepository.Atualizar(cliente);
            var clienteRecuperado = await _clienteRepository.ObterPorId(id);
            return clienteRecuperado ?? throw new Exception("Erro ao recuperar Cliente criado.");
        }

        public async Task<Cliente> DeletarCliente(long id) 
        {
            var cliente = await _clienteRepository.ObterPorId(id) 
                ?? throw new Exception($"Cliente com ID {id} não encontrado.");

            cliente.IsDeleted = true;
            cliente.Ativo = false;
            cliente.DataAtualizacao = DateTime.UtcNow;

            await _clienteRepository.Deletar(cliente);
            return cliente;

        }  

        public async Task<Cliente> ObterClientePorId(long id) 
        {
            var cliente = await _clienteRepository.ObterPorId(id) 
                ?? throw new Exception($"Cliente com ID {id} não encontrado.");

            return cliente;
        }

        public async Task<IEnumerable<Cliente>> ObterClientesPorEmpresa(long empresaId) 
        {
            var empresa = await _empresaRepository.ObterPorId(empresaId);
            if (empresa == null) throw new Exception($"Empresa com ID {empresaId} não encontrada.");

            var clientes = await _clienteRepository.ObterPorEmpresa(empresaId);
            if (clientes == null || !clientes.Any()) throw new Exception($"Empresa com ID {empresaId} não tem clientes cadastrados.");
            return clientes;
        }
    }
}
