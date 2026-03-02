namespace BarberFlow.API.DTOs.Servico
{
    public class ServicoCreateDto
    {
        public long EmpresaId { get; set; }
        public string Nome { get; set; }
        public int DuracaoMinutos { get; set; }
        public Decimal PrecoBase { get; set; }
    }
}
