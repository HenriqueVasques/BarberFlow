namespace BarberFlow.API.DTOs.Empresa
{
    public class EmpresaResponseDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public string CNPJ { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool IsDeleted { get; set; }
        public bool Ativo { get; set; }

    }
}
