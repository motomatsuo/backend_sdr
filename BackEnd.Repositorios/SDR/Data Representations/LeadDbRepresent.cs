
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using BackEnd.Modelos.SDR.Enums;

namespace BackEnd.Repositorios.SDR
{
    [Table("leads")]
    public class LeadDbRepresent : BaseModel
    {
        [PrimaryKey("lead_id", false)]
        public int LeadId { get; set; }

        [Column("razao_social")]
        public string RazaoSocial { get; set; }

        [Column("nome_fantasia")]
        public string NomeFantasia { get; set; }

        [Column("cnpj")]
        public string CNPJ { get; set; }

        [Column("setor")]
        public string Setor { get; set; }

        [Column("faturamento")]
        public string Faturamento { get; set; }

        [Column("site")]
        public string Site { get; set; }

        [Column("data_inclusao")]
        public DateTime DataInclusao{ get; set; }

        [Column("status_prospeccao_fk")]
        public int StatusProspeccaoFk { get; set; }

        [Column("login_portal_fk")]
        public int LoginPortalFk { get; set; }



        // public List<EnderecoLead> EnderecosLead { get; private set; } = new List<EnderecoLead>();
        // public List<ContatoLead> ContatosLead { get; private set; } = new List<ContatoLead>();
        // public List<LogAtividade> Atividades { get; private set; } = new List<LogAtividade>();
    }
}
