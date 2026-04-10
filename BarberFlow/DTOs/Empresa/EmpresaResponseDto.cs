namespace BarberFlow.API.DTOs.Empresa
{
    public class EmpresaResponseDto
    {
        #region Dados de Identificação
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public string CNPJ { get; set; }
        #endregion

        #region Status e Controle
        public bool Ativo { get; set; }
        #endregion

        #region Datas de Auditoria
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        #endregion
    }
}