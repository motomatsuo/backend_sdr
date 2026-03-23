using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Contact
{
    public record SimpleContactUpdateResponse(string? Nome, string? Cargo, string? Email);
}
