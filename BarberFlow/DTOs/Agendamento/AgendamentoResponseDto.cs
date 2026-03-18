using BarberFlow.API.Enums;

namespace BarberFlow.API.DTOs.Agendamento
{
    internal class AgendamentoResponseDto
    {
        public long Id { get; set; }
        public string NomeProfissional { get; set; }
        public string NomeCliente { get; set; }
        public string NomeServico { get; set; }
        public string NomeEmpresa { get; set; }
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        public long ClienteId { get; set; }
        public long ServicoId { get; set; }
        public decimal PrecoNoMomento { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public StatusAgendamento Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}