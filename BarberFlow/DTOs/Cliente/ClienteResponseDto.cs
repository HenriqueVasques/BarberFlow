namespace BarberFlow.API.DTOs.Cliente
{
    public class ClienteResponseDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Whatsapp { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool IsDeleted { get; set; }
        public bool Ativo { get; set; }
    }
}
