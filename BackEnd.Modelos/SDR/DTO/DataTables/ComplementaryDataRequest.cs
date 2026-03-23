using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.DataTables
{
    public record ComplementaryDataRequest
    (
        DateTime created_at,
        int id_dados_contato,
        bool comercializa_motos,
        string? tipo_moto,
        bool comercializa_peca,
        string? tipo_loja_peca,
        bool prestacao_servico,
        string? link_loja_online,
        string? tipo_prestacao_servico,
        string? como_conheceu,
        string? vendedor_indicado,
        string? modelo_empresa
    );
}
