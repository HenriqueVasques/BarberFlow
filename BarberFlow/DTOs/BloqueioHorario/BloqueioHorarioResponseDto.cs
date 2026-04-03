namespace BarberFlow.API.Controllers
{
    public class BloqueioHorarioResponseDto
    {
        #region Identificadores (Chaves e Relacionamentos)
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long ProfissionalId { get; set; }
        #endregion

        #region Informações de Exibição
        public string? NomeProfissional { get; set; }
        public string? NomeEmpresa { get; set; }
        public string Motivo { get; set; }
        #endregion

        #region Período e Auditoria
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        #endregion
    }
}