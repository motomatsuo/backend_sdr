using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Number
{
    public record NumberRepsonse
    (
        int NumeroId,
        string? Numero,
        string? Tipo,
        bool Whatsapp
    );
}
