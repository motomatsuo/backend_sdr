using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Repositorios.SDR
{
    /*
     O que eu preciso para usar a API do supabase + LIB
        - Intalar o pacote do supabase no projeto
        - Criar uma classe modelo que vai representar minha tabela no banco
        - Adicionar os decorators para a LIB do supabase mapear
        - Adicionar a URL e a API KEY appsetting.json
        - Fazer as configurações no program.cs
        - No controller fazer a consulta, que tem que voltar sempre um DTO
     */

    [Table("enderecos_lead")]
    public class AddressDbRepresent : BaseModel
    {
        [PrimaryKey("endereco_id", false)]
        public int EnderecoId { get; set; }

        [Column("rua")]
        public string? Rua { get; set; }

        [Column("numero")]
        public int Numero { get; set; }

        [Column("bairro")]
        public string? Bairro { get; set; }

        [Column("cidade")]
        public string? Cidade { get; set; }

        [Column("uf")]
        public string? UF { get; set; }

        [Column("lead_fk")]
        public int LeadFk { get; set; }
    }
}
