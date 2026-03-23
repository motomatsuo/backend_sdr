namespace BackEnd.Modelos.SDR.Modelos
{
    public class Note
    {
        public int NoteId { get; set; }
        public string? NoteData { get; set; }
        public int LeadId { get; set; }

        public Note(int noteId, string? noteData, int leadId)
        {
            NoteId = noteId;
            NoteData = noteData;
            LeadId = leadId;
        }
    }
}
