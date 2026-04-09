using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BackEnd.Repositorios.SDR.Data_Representations
{
    [Table("log_leads")]
    public class LogLeadsDbRepresent : BaseModel
    {
        [PrimaryKey("log_leads_id")]
        public int LogLeadsId { get; set; }

        [Column("log")]
        public string Log { get; set; }
        /*
        [Column("data_registro")]
        public DateTime? RegisterDate { get; set; }
        */
    }
}
