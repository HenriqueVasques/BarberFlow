namespace BarberFlow.API.DTOs.Servico
{
    public class ServicoUpdateDto
    {
        #region Properties
        public string Nome { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal PrecoBase { get; set; }
        public bool Ativo { get; set; }
        #endregion
    }
}
