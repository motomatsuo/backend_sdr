using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Modelos.SDR.DTO.Note
{
    public record NoteResponse
    (
        int NoteId,
        string NoteData,
        string CreationDate
    );
}
