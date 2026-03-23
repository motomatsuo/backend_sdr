using BackEnd.Modelos.SDR.DTO.Number;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Contact
{
    public record SimpleContactResponse
    (
        string? Nome,
        string? Cargo,
        string? Email,
        IEnumerable<SimpleNumberResponse>? Numeros
    );
}
