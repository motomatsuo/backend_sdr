using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BackEnd.Repositorios.SDR.Data_Representations
{
    [Table("db_primeiroacompanhamento_portal_sdr")]
    public class FirstMonitoringDbRepresent : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        /*
        [Column("cliente_comprou")]
        public string ClienteComprou { get; set; }

        [Column("comprou_vend")]
        public bool CommprouVend { get; set; }

        [Column("retorno")]
        public bool Retorno { get; set; }

        [Column("valor_compra")]
        public decimal ValorCompra { get; set; }

        [Column("motivo")]
        public string Motivo { get; set; }
        */
        [Column("data_acompanhamento")]
        public DateTime DataAcompanhamento { get; set; }
    }
}
