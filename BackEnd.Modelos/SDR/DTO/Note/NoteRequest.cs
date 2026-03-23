using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Note
{
    public record NoteRequest
    (
        string NoteData,
        int LeadId
    );
}
