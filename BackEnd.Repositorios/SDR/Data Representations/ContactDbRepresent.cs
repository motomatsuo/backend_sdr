using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BackEnd.Repositorios.SDR
{
    [Table("contatos_lead")]
    public class ContactDbRepresent : BaseModel
    {
        [PrimaryKey("contato_id")]
        public int ContatoId { get; set; }

        [Column("nome")]
        public string? Nome { get; set; }

        [Column("cargo")]
        public string? Cargo { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("lead_fk")]
        public int LeadFk { get; set; }
    }
}
