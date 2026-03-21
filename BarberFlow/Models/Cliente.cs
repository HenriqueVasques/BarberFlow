using System.Numerics;

namespace BarberFlow.API.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        public long EmpresaId { get; set; }
        public long UsuarioId { get; set; }
        public string Telefone { get; set; }
        public string Whatsapp { get; set; }
        public bool Ativo { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        //navegation properties
        public Empresa Empresa { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Agendamento> Agendamentos { get; set; }

        //segurança e privacidade
        public bool AceitouTermosPrivacidade { get; set; }
        public DateTime? DataConsentimento { get; set; }
        public string? IpConsentimento { get; set; }
        //Dica de Ouro: No link público de agendamento coloque um checkbox obrigatório:
        //"Aceito receber notificações de agendamento via WhatsApp e concordo com a Política de Privacidade". Salve o true e a Data no banco.

        public Cliente(long empresaId, long usuarioId, string telefone, string whatsapp)
        {
            EmpresaId = empresaId;
            UsuarioId = usuarioId;
            Telefone = telefone;
            Whatsapp = whatsapp;
            Ativo = true;
            IsDeleted = false;
            DataCriacao = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;
        }

    }
}
