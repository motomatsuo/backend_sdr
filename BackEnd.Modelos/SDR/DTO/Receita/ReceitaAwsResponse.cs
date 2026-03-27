using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Receita
{
    public record ReceitaAwsResponse
    (
        string? RazaoSocial,
        string? Cnpj,
        string? CodigoAtividade,
        string? Situacao,
        string? Porte,
        string? DataAbertura,
        string? Logradouro,
        string? Bairro,
        string? Municipio,
        string? Uf,
        string? Cep
    );
}
