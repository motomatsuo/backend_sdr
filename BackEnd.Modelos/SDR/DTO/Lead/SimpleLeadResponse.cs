using BackEnd.Modelos.SDR.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Lead
{
    public record SimpleLeadResponse
    (
        int LeadId,
        string RazaoSocial,
        string? NomeFantasia,
        string CNPJ,
        string? Setor,
        string Faturamento,
        string? Site,
        string DataInclusao,
        string StatusProspeccaoFk,
        int IdLoginPortal
    );
}
