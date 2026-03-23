using BackEnd.Servicos.SDR.Services;
using Microsoft.AspNetCore.Mvc;
using Supabase;

namespace BackEnd.API.Controllers.SDR
{
    [ApiController]
    [Route("api/nota")]
    public class NotesController : ControllerBase
    {
        private readonly Client _supabase;
        private NoteService _noteService;

        public NotesController(Client supabase, NoteService noteService)
        {
            _supabase = supabase;
            _noteService = noteService;
        }

        [HttpGet("{idLead}")]
        public async Task<IActionResult> GetNotes(int idLead)
        {
            try
            {
                var notes = await _noteService.GetNotesByLeadId(idLead);
                return Ok(notes);
            }
            catch (Exception)
            {
                return BadRequest("Houve um erro ao consultar as notas geradas.");
            }
        }

        [HttpPost("{idLead}")]
        public async Task<IActionResult> CreateNote(int idLead, [FromBody] string noteContent)
        {
            try
            {
                // Implementar lógica para criar uma nova nota para o lead com idLead
                // Por exemplo, você pode criar um método no NoteService para adicionar uma nova nota
                await _noteService.CreateNoteForLead(idLead, noteContent); // Placeholder, substituir pela lógica correta
                return Ok("Nota criada com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest("Houve um erro ao criar a nota.");
            }
        }

        [HttpPut("{idNote}")]
        public async Task<IActionResult> UpdateNote(int idNote, [FromBody] string noteContent)
        {
            try
            {
                // Implementar lógica para atualizar uma nota existente para o lead com idLead
                // Por exemplo, você pode criar um método no NoteService para atualizar a nota
                var updateNote = await _noteService.UpdateNoteForLead(idNote, noteContent); // Placeholder, substituir pela lógica correta
                return Ok(updateNote);
            }
            catch (Exception)
            {
                return BadRequest("Houve um erro ao atualizar a nota.");
            }
        }

        [HttpDelete("{idNote}")]
        public async Task<IActionResult> DeleteNote(int idNote)
        {
            try
            {
                // Implementar lógica para deletar uma nota existente para o lead com idLead
                // Por exemplo, você pode criar um método no NoteService para deletar a nota
                _noteService.DeleteNote(idNote); // Placeholder, substituir pela lógica correta
                return Ok("Nota deletada com sucesso.");
            }
            catch (Exception)
            {
                return BadRequest("Houve um erro ao deletar a nota.");
            }
        }
    }
}
