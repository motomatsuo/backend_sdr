using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Lead
{
    public record LeadDeleteReasonRequest(string LeadName, string Cnpj, string ColumnStatus, string Attendant, string Reason);
}
