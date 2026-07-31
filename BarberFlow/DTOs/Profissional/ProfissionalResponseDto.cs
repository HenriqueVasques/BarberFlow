namespace BarberFlow.API.DTOs.Profissional
{
    public class ProfissionalResponseDto
    {
        #region Identificação
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        #endregion

        #region Dados Cadastrais e Perfil
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NomeEmpresa { get; set; } = string.Empty;
        public decimal PercentualComissao { get; set; }
        #endregion

        #region Status e Auditoria
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        #endregion
    }
}