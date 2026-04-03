namespace BarberFlow.API.DTOs.Cliente
{
    public class ClienteResponseDto
    {
        #region Identificadores e Vínculos
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        #endregion

        #region Informações de Contato
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Whatsapp { get; set; }
        #endregion

        #region Status e Auditoria
        public bool Ativo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        #endregion
    }
}