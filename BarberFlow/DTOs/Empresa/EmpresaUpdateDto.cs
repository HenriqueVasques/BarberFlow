namespace BarberFlow.API.DTOs.Empresa
{
    public class EmpresaUpdateDto
    {
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public bool? Ativo { get; set; }
    }
}
