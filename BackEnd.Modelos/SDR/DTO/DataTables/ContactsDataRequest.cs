using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.DataTables
{
    public record ContactsDataRequest
    (
        string nome_cliente,
        string? telefone,
        string? celular,
        string? email,
        string cnpj,
        string? razao_social,
        string? situacao,
        string? atividade,
        string socios,
        string? logradouro,
        string? bairro,
        string? municipio,
        string? uf,
        string? cep
    );
}