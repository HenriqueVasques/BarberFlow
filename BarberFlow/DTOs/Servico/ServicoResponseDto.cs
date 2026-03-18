namespace BarberFlow.API.DTOs.Servico
{
    public class ServicoResponseDto
    {
        #region Properties
        public long Id { get; set; }
        public string Nome { get; set; }
        public string NomeEmpresa { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal PrecoBase { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Ativo { get; set; } = true;
        #endregion
    }
}
