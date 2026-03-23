using BackEnd.Modelos.SDR.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Lead
{
    public record UpdateLeadRequest
    (
         string NomeFantasia,
         string Setor,
         string Faturamento,
         string Site
    );
}
