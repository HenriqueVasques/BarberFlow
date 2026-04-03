using BarberFlow.API.Data.Context;
using BarberFlow.API.DTOs.Agendamento;
using BarberFlow.API.Enums;
using BarberFlow.API.Interfaces;
using BarberFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberFlow.API.Data.Repositories 
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        #region Readonly Fields
        private readonly AppDbContext _appDbContext;
        #endregion

        #region Construtor
        public AgendamentoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Comandos (Escrita)

        // Insere um novo agendamento no banco
        public async Task Adicionar(Agendamento agendamento)
        {
            await _appDbContext.Agendamentos.AddAsync(agendamento);
            await _appDbContext.SaveChangesAsync();
        }

        // Atualiza dados de um agendamento (Ex: Alterar Status)
        public async Task Atualizar(Agendamento agendamento)
        {
            _appDbContext.Agendamentos.Update(agendamento);
            await _appDbContext.SaveChangesAsync();
        }

        #endregion

        #region Consultas: Visão Geral

        // Busca completa de um agendamento por ID com todos os relacionamentos
        public async Task<Agendamento?> ObterPorId(long id)
        {
            return await _appDbContext.Agendamentos
                .IgnoreQueryFilters()
                .Include(a => a.Empresa)
                .Include(a => a.ProfissionalServico).ThenInclude(p => p.Servico)
                .Include(a => a.ProfissionalServico).ThenInclude(p => p.Profissional).ThenInclude(p => p.Usuario)
                .Include(a => a.Cliente).ThenInclude(c => c.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Busca agenda filtrada por período (Pode ser de um profissional específico ou da empresa toda)]
            public async Task<List<AgendamentoResponseDto>> ObterAgendaPorPeriodo(long? profissionalId, long empresaId, DateOnly inicio, DateOnly fim, List<StatusAgendamento> statusFiltro, int pagina = 1)
            {
                var query = _appDbContext.Agendamentos
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(a => a.EmpresaId == empresaId &&
                                a.DataHoraInicio >= inicio.ToDateTime(TimeOnly.MinValue) &&
                                a.DataHoraInicio <= fim.ToDateTime(TimeOnly.MaxValue) &&
                                statusFiltro.Contains(a.Status)
                    );

                if (profissionalId.HasValue && profissionalId > 0)
                {
                    query = query.Where(a => a.ProfissionalServico.ProfissionalId == profissionalId);
                }

                return await query
                    .OrderBy(a => a.DataHoraInicio)
                    .Select(a => new AgendamentoResponseDto
                    {
                        Id = a.Id,
                        EmpresaId = a.EmpresaId,
                        ClienteId = a.ClienteId,
                        ServicoId = a.ProfissionalServico.ServicoId,
                        ProfissionalId = a.ProfissionalServico.ProfissionalId,
                        ProfissionalServicoId = a.ProfissionalServicoId,
                        DataHoraInicio = a.DataHoraInicio,
                        DataHoraFim = a.DataHoraFim,
                        Status = a.Status,
                        PrecoNoMomento = a.PrecoNoMomento,
                        NomeEmpresa = a.Empresa.Nome,
                        NomeCliente = a.Cliente.Usuario.Nome,
                        NomeProfissional = a.ProfissionalServico.Profissional.Usuario.Nome,
                        NomeServico = a.ProfissionalServico.Servico.Nome,
                        DataAtualizacao = a.DataAtualizacao,
                        DataCriacao = a.DataCriacao
                    })
                    .Skip((pagina - 1) * 10)
                    .Take(10)
                    .ToListAsync();
            }

        #endregion

        #region Consultas: Dashboards (Admin)

        // Soma o valor total de agendamentos confirmados/finalizados no dia
        public async Task<decimal> ObterFaturamentoPorDia(long empresaId, DateOnly data)
        {
            var inicioDia = data;
            var fimDia = data;

            return await _appDbContext.Agendamentos
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(a => a.EmpresaId == empresaId &&
                            a.DataHoraInicio >= inicioDia.ToDateTime(TimeOnly.MinValue) &&
                            a.DataHoraInicio <= fimDia.ToDateTime(TimeOnly.MaxValue) &&
                            (a.Status == StatusAgendamento.Confirmado || a.Status == StatusAgendamento.Finalizado))
                .SumAsync(a => a.PrecoNoMomento);
        }

        // Conta a quantidade de atendimentos realizados no dia
        public async Task<int> ContarAgendamentoPorDia(long empresaId, DateOnly data)
        {
            var inicioDia = data;
            var fimDia = data;

            return await _appDbContext.Agendamentos
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(a => a.EmpresaId == empresaId &&
                            a.DataHoraInicio >= inicioDia.ToDateTime(TimeOnly.MinValue) &&
                            a.DataHoraInicio <= fimDia.ToDateTime(TimeOnly.MaxValue) &&
                            (a.Status == StatusAgendamento.Confirmado || a.Status == StatusAgendamento.Finalizado))
                .CountAsync();
        }

        #endregion

        #region Consultas: Cliente

        // Localiza o compromisso mais próximo que o cliente ainda irá realizar
        public async Task<AgendamentoResponseDto?> ObterProximoAgendamentoCliente(long clienteId)
        {
            return await _appDbContext.Agendamentos
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(a => a.ClienteId == clienteId &&
                            a.DataHoraInicio >= DateTime.UtcNow &&
                            (a.Status == StatusAgendamento.Confirmado || a.Status == StatusAgendamento.Pendente))
                .OrderBy(a => a.DataHoraInicio)
                .Select(a => new AgendamentoResponseDto
                {
                    Id = a.Id,
                    ClienteId = a.ClienteId,
                    ProfissionalId = a.ProfissionalServico.ProfissionalId,
                    ServicoId = a.ProfissionalServico.ServicoId,
                    ProfissionalServicoId = a.ProfissionalServicoId,
                    DataHoraInicio = a.DataHoraInicio,
                    DataHoraFim = a.DataHoraFim,
                    Status = a.Status,
                    PrecoNoMomento = a.PrecoNoMomento,
                    NomeEmpresa = a.Empresa.Nome,
                    NomeCliente = a.Cliente.Usuario.Nome,
                    NomeProfissional = a.ProfissionalServico.Profissional.Usuario.Nome,
                    NomeServico = a.ProfissionalServico.Servico.Nome,
                    DataAtualizacao = a.DataAtualizacao,
                    DataCriacao = a.DataCriacao
                })
                .FirstOrDefaultAsync();
        }

        // Traz os últimos registros do cliente (Histórico de visitas)
        public async Task<List<AgendamentoResponseDto>> ObterUltimosAgendamentosPorCliente(long clienteId, int quantidade)
        {
            return await _appDbContext.Agendamentos
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(a => a.ClienteId == clienteId)
                .OrderByDescending(a => a.DataHoraInicio)
                .Take(quantidade)
                .Select(a => new AgendamentoResponseDto
                {
                    Id = a.Id,
                    ClienteId = a.ClienteId,
                    ProfissionalId = a.ProfissionalServico.ProfissionalId,
                    ServicoId = a.ProfissionalServico.ServicoId,
                    ProfissionalServicoId = a.ProfissionalServicoId,
                    DataHoraInicio = a.DataHoraInicio,
                    DataHoraFim = a.DataHoraFim,
                    Status = a.Status,
                    PrecoNoMomento = a.PrecoNoMomento,
                    NomeEmpresa = a.Empresa.Nome,
                    NomeCliente = a.Cliente.Usuario.Nome,
                    NomeProfissional = a.ProfissionalServico.Profissional.Usuario.Nome,
                    NomeServico = a.ProfissionalServico.Servico.Nome,
                    DataAtualizacao = a.DataAtualizacao,
                    DataCriacao = a.DataCriacao
                })
                .ToListAsync();
        }

        #endregion

        #region Validações de Conflito (Blindagem)

        // Verifica se o profissional está livre (Checa tanto agendamentos quanto bloqueios manuais)
        public async Task<bool> EstaOcupado(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null, long? bloqueioIdParaIgnorar = null)
        {
            var temConflito = await TemConflitoAgendamento(profissionalId, inicio, fim, agendamentoIdParaIgnorar) ||
                              await TemConflitoBloqueio(profissionalId, inicio, fim, bloqueioIdParaIgnorar) ||
                              await EstaForaDoHorarioTrabalho(profissionalId, inicio, fim);

            return temConflito;
        }

        // Valida sobreposição com bloqueios de horário (Ex: Almoço, Folga)
        public async Task<bool> TemConflitoBloqueio(long profissionalId, DateTime inicio, DateTime fim, long? bloqueioIdParaIgnorar = null)
        {
            return await _appDbContext.BloqueioHorarios
                .AnyAsync(b => b.ProfissionalId == profissionalId &&
                               (bloqueioIdParaIgnorar == null || b.Id != bloqueioIdParaIgnorar) &&
                               !b.IsDeleted &&
                               inicio < b.DataHoraFim && fim > b.DataHoraInicio
                );
        }

        // Valida sobreposição com outros agendamentos já marcados
        public async Task<bool> TemConflitoAgendamento(long profissionalId, DateTime inicio, DateTime fim, long? agendamentoIdParaIgnorar = null)
        {
            return await _appDbContext.Agendamentos
                .AnyAsync(a => a.ProfissionalServico.ProfissionalId == profissionalId &&
                               (agendamentoIdParaIgnorar == null || a.Id != agendamentoIdParaIgnorar) &&
                               a.Status != StatusAgendamento.Cancelado &&
                               inicio < a.DataHoraFim && fim > a.DataHoraInicio
                );
        }

        public async Task<bool> EstaForaDoHorarioTrabalho(long profissionalId, DateTime inicio, DateTime fim)
        {
            var horaInicioAgendamento = TimeOnly.FromDateTime(inicio);
            var horaFimAgendamento = TimeOnly.FromDateTime(fim);

            // Verificamos se EXISTE um horário que CUBRA o agendamento
            var possuiHorarioCompativel = await _appDbContext.HorarioProfissionais
                .AnyAsync(hp => hp.ProfissionalId == profissionalId &&
                                hp.DiaSemana == inicio.DayOfWeek &&
                                hp.Ativo && !hp.IsDeleted &&
                                hp.HoraInicio <= horaInicioAgendamento &&
                                hp.HoraFim >= horaFimAgendamento &&
                                !(horaInicioAgendamento < hp.HoraFimAlmoco && horaFimAgendamento > hp.HoraInicioAlmoco)
                );

            // Se possuiHorarioCompativel for false, retorna true (tem conflito/está fora)
            return !possuiHorarioCompativel;
        }

        #endregion
    }
}