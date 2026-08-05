namespace BarberFlow.API.DTOs.Servico
{
    public class ServicoResponseDto
    {
        #region Identificação
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NomeEmpresa { get; set; } = string.Empty;
        #endregion

        #region Configurações Base
        public int DuracaoMinutos { get; set; }
        public decimal PrecoBase { get; set; }
        #endregion

        #region Status e Auditoria
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        #endregion
    }
}