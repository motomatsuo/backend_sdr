using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Lead
{
    public record UpdatedLeadResponse
    (
         string NomeFantasia,
         string Setor,
         string Faturamento,
         string Site
    );
}
