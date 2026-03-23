using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BackEnd.Repositorios.SDR.Data_Representations
{
    [Table("log_atividades")]
    public class ActivityLogDbRepresent : BaseModel
    {
        [PrimaryKey("log_atividade_id")]
        public int ActicityLogId { get; set; }

        [Column("registro")]
        public DateTime Register{ get; set; }

        [Column("descricao")]
        public string Description { get; set; }

        [Column("status_prospeccao_fk")]
        public int StatusProspeccaoFk { get; set; }

        [Column("lead_fk")]
        public int LeadFk{ get; set; }

        [Column("login_portal_fk")]
        public int LoginPortalFk{ get; set; }
    }
}
