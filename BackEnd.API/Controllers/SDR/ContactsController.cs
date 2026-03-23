using BackEnd.Modelos.SDR.DTO;
using BackEnd.Modelos.SDR.DTO.Contact;
using BackEnd.Modelos.SDR.DTO.Number;
using BackEnd.Modelos.SDR.Exceptions;
using BackEnd.Repositorios.SDR;
using BackEnd.Repositorios.SDR.Exceptions;
using BackEnd.Servicos.SDR.Services;
using Microsoft.AspNetCore.Mvc;
using Supabase;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackEnd.Controllers.SDR
{
    /// <summary>
    /// Padrão de nomeclatura dos métodos e mapeamento:
    /// 
    /// Camada     -> | Controller | Serviço   | Repositório |
    ///               |            |           |             |
    /// --------------|------------|-----------|-------------|
    /// Mapeamento -> | GET        | Retrieve  | Select      |
    /// Mapeamento -> | POST       | Register  | Insert      |
    /// Mapeamento -> | PUT        | Modify    | Update      |
    /// Mapeamento -> | DELETE     | Remove    | Delete      |
    /// 
    /// </summary>

    [ApiController]
    [Route("api/contato")]
    public class ContactsController : ControllerBase
    {
        private readonly Client _supabase;
        private ContactService _contactService;

        public ContactsController(Client supabase, ContactService contactService)
        {
            _supabase = supabase;
            _contactService = contactService;
        }

        // ------------------------------------- MÉTODOS GET REFATORADOS E CORRETOS -------------------------------------


        [HttpGet]
        public async Task<IActionResult> ObterContato()
        {
            var contatoDbResponse = await _supabase
                .From<ContactDbRepresent>()
                .Order(contato => contato.ContatoId, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();


            var contatosList = new List<ContactResponse>();

            foreach (var c in contatoDbResponse.Models)
            {
                var numeroDbResponse = await _supabase
                    .From<NumberDbRepresent>()
                    .Where(n => n.ContatoFk == c.ContatoId)
                    .Get();

                var numeros = numeroDbResponse.Models
                    .Select(n => new NumberRepsonse(n.NumeroId, n.Numero, n.Tipo, n.Whatsapp))
                    .ToList(); // ContatoResponse aceita IEnumerable, List serve.

                contatosList.Add(new ContactResponse(c.ContatoId, c.Nome, c.Cargo, c.Email, numeros));
            }

            return Ok(contatosList);
        }

        [HttpGet("{idLead}")]
        public async Task<IActionResult> ObterContatoEspecifico(int idLead)
        {
            var contatoDbResponse = await _supabase
                .From<ContactDbRepresent>()
                .Where(c => c.LeadFk == idLead)
                .Order(contato => contato.ContatoId, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();


            var contatosList = new List<ContactResponse>();

            foreach (var c in contatoDbResponse.Models)
            {
                var numeroDbResponse = await _supabase
                    .From<NumberDbRepresent>()
                    .Where(n => n.ContatoFk == c.ContatoId)
                    .Get();

                var numeros = numeroDbResponse.Models
                    .Select(n => new NumberRepsonse(n.NumeroId, n.Numero, n.Tipo, n.Whatsapp))
                    .ToList(); // ContatoResponse aceita IEnumerable, List serve.

                contatosList.Add(new ContactResponse(c.ContatoId, c.Nome, c.Cargo, c.Email, numeros));
            }

            return Ok(contatosList);
        }

        // ------------------------------------- MÉTODOS POST REFATORADOS E CORRETOS -------------------------------------


        [HttpPost("{idLead}")]
        public async Task<IActionResult> PostContato([FromBody] List<CreateContactsRequest> createContactsRequestList, int idLead)
        {
            try
            {
                var contacts = await _contactService.CadastrarContatos(createContactsRequestList, idLead);
                return Ok(contacts.ToList());
            }
            catch (Exception)
            {

                return BadRequest("Erro ao cadastrar contatos.");
            }
        }


        [HttpPost("numero/{contactId}")]
        public async Task<IActionResult> PostContactNumberAsync(int contactId, [FromBody] CreateNumberRequest createNumber)
        {
            try
            {
                var response = await _contactService.RegisterLeadContatcNumberAsync(contactId, createNumber);
                return Ok(response);
            }
            catch (RepositoriesException ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message} Por favor procure o suporte.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message} Por favor procure o suporte.");
            }
        } // Completo

        // ------------------------------------- MÉTODOS PUT REFATORADOS E CORRETOS -------------------------------------

        [HttpPut("{contactId}")]
        public async Task<IActionResult> PutContactAsync(int contactId, [FromBody] UpdateContactRequest updateContactRequest)
        {
            try
            {
                var updatedContact = await _contactService.ModifyLeadContactAsync(contactId, updateContactRequest);
                return Ok(updatedContact);
            }
            catch (RepositoriesException ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message} Por favor procure o suporte.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message} Por favor procure o suporte.");
            }
        } // Completo

        [HttpPut("numero/{numberId}")]
        public async Task<IActionResult> PutContactNumberAsync(int numberId, [FromBody] UpdateNumberRequest updateNumber)
        {
            try
            {
                var updatedNumber = await _contactService.ModifyLeadContactNumberAsync(numberId, updateNumber);
                return Ok(updatedNumber);
            }
            catch (RepositoriesException ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message} Por favor procure o suporte.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro: {ex.Message} Por favor procure o suporte.");
            }
        } // Completo

        // ------------------------------------- MÉTODOS DELETE REFATORADOS E CORRETOS -------------------------------------

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            // Verifica se existe
            var existing = await _supabase
                .From<ContactDbRepresent>()
                .Where(c => c.ContatoId == id)
                .Get();

            if (!existing.Models.Any())
                return NotFound("Contato não encontrado.");

            await _supabase
                .From<ContactDbRepresent>()
                .Where(c => c.ContatoId == id)
                .Delete();

            return NoContent(); // 204
        }



        
    }
}
