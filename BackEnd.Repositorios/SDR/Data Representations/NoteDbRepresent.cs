using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BackEnd.Repositorios.SDR.Data_Representations
{
    [Table("notas_lead")]
    public class NoteDbRepresent : BaseModel
    {
        [PrimaryKey("nota_id")]
        public int NoteId { get; set; }

        [Column("nota")]
        public string? Note { get; set; }

        [Column("data_criacao")]
        public DateTime CreationDate { get; set; }

        [Column("lead_fk")]
        public int LeadFk { get; set; }
    }
}
