namespace BarberFlow.API.DTOs.Servico
{
    public class ServicoResponseDto
    {
        #region Properties
        public string Nome { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal PrecoBase { get; set; }
        public DateTime DataCriacao { get; set; }
        #endregion
    }
}
