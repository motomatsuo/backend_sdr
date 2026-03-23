using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Address
{
    public record SimpleAddressResponse
    (
        string? Rua,
        int Numero,
        string? Bairro,
        string? Cidade,
        string? Uf
    );
}
