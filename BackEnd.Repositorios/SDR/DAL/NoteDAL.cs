using BackEnd.Modelos.SDR.DTO.Note;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Repositorios.SDR.Data_Representations;
using Supabase;


namespace BackEnd.Repositorios.SDR.DAL
{
    public class NoteDAL
    {
        private Client _supabase;

        public NoteDAL(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<HashSet<NoteResponse>> SelectAllNotesByLeadId(int idLead) // Vai trazer todas as notas existentes: FUNCIONANDO
        {
            var noteDbResponse = await _supabase
                .From<NoteDbRepresent>()
                .Where(n => n.LeadFk == idLead)
                .Order(n => n.NoteId, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            var noteList = new HashSet<NoteResponse>();
            foreach (var n in noteDbResponse.Models)
            {
                noteList.Add(new NoteResponse(n.NoteId, n.Note, n.CreationDate.ToString("g")));
            }
            return noteList;
        }

        public async Task InsertNotesForLead(int idLead, string noteContent)
        {
            var noteDb = new NoteDbRepresent
            {
                Note = noteContent,
                CreationDate = DateTime.Now,
                LeadFk = idLead
            };

            var response = await _supabase
                .From<NoteDbRepresent>()
                .Insert(noteDb);

            var created = response.Models.FirstOrDefault();

            if (created == null)
                throw new Exception("Erro ao inserir nota.");
        }

        public async Task<NoteResponse> UpdateNoteForLead(int idNote, string noteContent)
        {
            var noteDb = new NoteDbRepresent
            {
                Note = noteContent,
            };

            var response = await _supabase
                .From<NoteDbRepresent>()
                .Where(n => n.NoteId == idNote)
                .Set(n => n.Note, noteContent)
                .Update();

            var updated = response.Models.FirstOrDefault();

            if (updated == null)
                throw new Exception("Erro ao atualizar nota.");

            return new NoteResponse(updated.NoteId, updated.Note, updated.CreationDate.ToString("g"));
        }

        public async Task DeleteNoteForLead(int idNote)
        {
            await _supabase
                .From<NoteDbRepresent>()
                .Where(n => n.NoteId == idNote)
                .Delete();
        }
    }
}
