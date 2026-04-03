using BarberFlow.API.Enums;

namespace BarberFlow.API.DTOs.Agendamento
{
    public class AgendamentoResponseDto
    {
        #region Identificadores (Chaves e Relacionamentos)
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long ClienteId { get; set; }
        public long ServicoId { get; set; }
        public long ProfissionalId { get; set; }
        public long ProfissionalServicoId { get; set; }
        #endregion

        #region Informações de Exibição (Nomes)
        public string NomeProfissional { get; set; }
        public string NomeCliente { get; set; }
        public string NomeServico { get; set; }
        public string NomeEmpresa { get; set; }
        #endregion

        #region Valores e Status
        public decimal PrecoNoMomento { get; set; }
        public StatusAgendamento Status { get; set; }
        public string StatusDescricao => Status.ToString();
        #endregion

        #region Datas e Auditoria
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        #endregion
    }
}