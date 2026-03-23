using BackEnd.Modelos.SDR.DTO.Number;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Contact
{
    public record CreateContactsRequest
    (
        string? Nome,
        string? Cargo,
        string? Email,
        List<CreateNumberRequest>? Numeros
    );
}
