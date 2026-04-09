using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Empresa;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly AppDbContext _appDbContext;

        public EmpresaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Operações de Escrita (Comandos)

        // Adiciona uma nova empresa e persiste as alterações no banco
        public async Task Adicionar(Empresa empresa)
        {
            await _appDbContext.Empresas.AddAsync(empresa);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza os dados de uma empresa existente
        public async Task Atualizar(Empresa empresa)
        {
            _appDbContext.Empresas.Update(empresa);
            await _appDbContext.SaveChangesAsync();
        }

        // Realiza a atualização do estado da empresa (usado para exclusão lógica)
        public async Task Deletar(Empresa empresa)
        {
            _appDbContext.Empresas.Update(empresa);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Operações de Leitura (Consultas)

        // Verifica se um slug já está em uso por alguma empresa ativa
        public async Task<bool> ExisteSlug(string slug)
        {
            return await _appDbContext.Empresas.AnyAsync(e => e.Slug == slug && !e.IsDeleted);
        }

        // Verifica se um CNPJ já existe na base de dados (ignora empresas deletadas)
        public async Task<bool> ExisteCnpj(string cnpj)
        {
            return await _appDbContext.Empresas.AnyAsync(e => e.CNPJ == cnpj && !e.IsDeleted);
        }

        // Retorna a entidade Empresa com filtros flexíveis de status
        public async Task<Empresa?> ObterPorId(long id, bool apenasAtivos = true, bool incluirDeletados = false)
        {
            return await _appDbContext.Empresas
                .IgnoreQueryFilters()
                .Where(e => e.Id == id &&
                           (incluirDeletados || !e.IsDeleted) &&
                           (apenasAtivos || e.Ativo))
                .FirstOrDefaultAsync();
        }

        // Busca a empresa e inclui a lista de horários de funcionamento vinculados
        public async Task<Empresa?> ObterPorIdComHorarioEmpresa(long id)
        {
            return await _appDbContext.Empresas
                .Include(e => e.HorariosFuncionamentoEmpresa)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted && e.Ativo);
        }

        // Projeta os dados da empresa diretamente para o DTO de resposta via Slug
        public async Task<EmpresaResponseDto?> ObterPorSlug(string slug)
        {
            return await _appDbContext.Empresas
                .AsNoTracking()
                .Where(e => e.Slug == slug && !e.IsDeleted && e.Ativo)
                .Select(e => new EmpresaResponseDto
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    CNPJ = e.CNPJ,
                    Slug = e.Slug,
                    DataCriacao = e.DataCriacao,
                    DataAtualizacao = e.DataAtualizacao
                })
                .FirstOrDefaultAsync();
        }

        #endregion
    }
}