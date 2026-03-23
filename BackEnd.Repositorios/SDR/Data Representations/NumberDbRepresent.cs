using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BackEnd.Repositorios.SDR
{
    [Table("numeros_lead")]
    public class NumberDbRepresent : BaseModel
    {
        [PrimaryKey("numero_id")]
        public int NumeroId { get; set; }

        [Column("numero")]
        public string? Numero { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("whatsapp")]
        public bool Whatsapp { get; set; }

        [Column("contato_fk")]
        public int ContatoFk { get; set; }
    }
}

