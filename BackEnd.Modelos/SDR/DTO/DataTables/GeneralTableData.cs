using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.DataTables
{
    public record GeneralTableData(DateTime created_at, int dados_contato, int dados_gerais, int primeiro_acompanhamento, int segundo_acompanhamento, int terceiro_acompanhamento, int dados_auxiliar);
}
