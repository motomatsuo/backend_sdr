using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Contact
{
    public record UpdatedContactResponse(string? Nome, string? Cargo, string? Email);
}
