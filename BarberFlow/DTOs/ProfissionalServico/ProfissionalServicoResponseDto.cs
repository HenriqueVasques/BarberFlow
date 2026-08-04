namespace BarberFlow.API.DTOs.ProfissionalServico
{
    public class ProfissionalServicoResponseDto
    {
        #region Identificação
        public long Id { get; set; }
        public long ProfissionalId { get; set; }
        public long ServicoId { get; set; }
        #endregion

        #region Descrição do Vínculo
        public string NomeServico { get; set; } = string.Empty;
        public string NomeProfissional { get; set; } = string.Empty;
        #endregion

        #region Configurações Personalizadas
        public decimal? PrecoPersonalizado { get; set; }
        public int? DuracaoPersonalizadaMinutos { get; set; }
        #endregion

        #region Status e Auditoria
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        #endregion
    }
}