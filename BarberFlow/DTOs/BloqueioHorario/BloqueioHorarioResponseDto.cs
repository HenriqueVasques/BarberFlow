
namespace BarberFlow.API.Controllers
{
    internal class BloqueioHorarioResponseDto
    {
        public long Id { get; set; }
        public string NomeProfissional { get; set; }
        public string NomeEmpresa { get; set; }
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Motivo { get; set; }
    }
}