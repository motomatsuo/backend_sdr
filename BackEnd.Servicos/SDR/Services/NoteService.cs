using BackEnd.Modelos.SDR.DTO.Note;
using BackEnd.Repositorios.SDR.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Servicos.SDR.Services
{
    public class NoteService
    {
        private readonly NoteDAL _noteDAL;

        public NoteService(NoteDAL noteDAL)
        {
            _noteDAL = noteDAL;
        }

        public async Task<HashSet<NoteResponse>> GetNotesByLeadId(int idLead)
        {
            // Implementar lógica para obter notas específicas com base no idLead
            // Por exemplo, você pode criar um método no NoteDAL para buscar as notas pelo idLead
            return await _noteDAL.SelectAllNotesByLeadId(idLead); // Placeholder, substituir pela lógica correta
        }

        public async Task CreateNoteForLead(int idLead, string noteContent)
        {
            // Implementar lógica para criar uma nova nota para o lead com idLead
            // Por exemplo, você pode criar um método no NoteDAL para adicionar uma nova nota
            await _noteDAL.InsertNotesForLead(idLead, noteContent); // Placeholder, substituir pela lógica correta
        }

        public async Task<NoteResponse> UpdateNoteForLead(int idNote, string noteContent)
        {
            // Implementar lógica para atualizar uma nota existente para o lead com idLead
            // Por exemplo, você pode criar um método no NoteDAL para atualizar a nota
            return await _noteDAL.UpdateNoteForLead(idNote, noteContent); // Placeholder, substituir pela lógica correta
        }

        public void DeleteNote(int idNote)
        {
            // Implementar lógica para deletar uma nota existente para o lead com idLead
            // Por exemplo, você pode criar um método no NoteDAL para deletar a nota
             _noteDAL.DeleteNoteForLead(idNote); // Placeholder, substituir pela lógica correta
        }
    }
}
