namespace BarberFlow.API.DTOs.Servico
{
    public class ServicoResponseDto
    {
        #region Properties
        public string Nome { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal PrecoBase { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool IsDeleted { get; set; }
        public bool Ativo { get; set; }
        #endregion
    }
}
