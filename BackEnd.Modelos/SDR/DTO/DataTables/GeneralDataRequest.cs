using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.DataTables
{
    public record GeneralDataRequest
    (
        string campanha,
        string anuncio,
        string plataforma,
        DateTime criado_em,
        string id_cliente,
        string status
    );
}
