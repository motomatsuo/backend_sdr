using BackEnd.Modelos.SDR.DTO.Address;
using BackEnd.Modelos.SDR.DTO.Contact;
using BackEnd.Modelos.SDR.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Lead
{
    public record CreateLeadRequest
    (
        string RazaoSocial,
        string? NomeFantasia,
        string CNPJ,
        string? Setor,
        string Faturamento,
        string? Site,
        IEnumerable<CreateAddressRequest>? Enderecos,
        IEnumerable<CreateContactsRequest>? Contatos
    );
}
