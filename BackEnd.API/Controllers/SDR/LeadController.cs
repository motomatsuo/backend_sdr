using BackEnd.Modelos.SDR.DTO;
using BackEnd.Modelos.SDR.DTO.Address;
using BackEnd.Modelos.SDR.DTO.Contact;
using BackEnd.Modelos.SDR.DTO.DataTables;
using BackEnd.Modelos.SDR.DTO.Lead;
using BackEnd.Modelos.SDR.DTO.Number;
using BackEnd.Modelos.SDR.Enums;
using BackEnd.Modelos.SDR.Exceptions;
using BackEnd.Repositorios.SDR;
using BackEnd.Repositorios.SDR.Exceptions;
using BackEnd.Servicos.SDR.Services;
using Microsoft.AspNetCore.Mvc;
using Supabase;
using Supabase.Postgrest.Exceptions;

namespace BackEnd.API.Controllers.SDR
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
    [Route("api/lead")]
    public class LeadController : ControllerBase
    {
        private readonly Client _supabase;
        private LeadService _leadService;
        private readonly SdrService _sdrService;

        public LeadController(Client supabase, LeadService leadService, SdrService sdrService)
        {
            _supabase = supabase;
            _leadService = leadService;
            _sdrService = sdrService;
        }


        // ------------------------------------- MÉTODOS GET REFATORADOS E CORRETOS -------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetLeads()
        {
            var leads = await _leadService.RetrieveAllLeads();
            return Ok(leads);
        } // Completo

        [HttpGet("{idLead}")]
        public async Task<IActionResult> GetSpecificLead(int idLead)
        {
            try
            {
                LeadResponse leadRespose = await _leadService.RetrieveSpecificLeadById(idLead);
                return Ok(leadRespose);
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro na consulta: {ex.Message}");
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
        } // Completo

        [HttpGet("{idLead}/activity")]
        public async Task<IActionResult> GetCadencyStartDate(int idLead)
        {
            try
            {
                DateTime cadencyStartDate = await _leadService.RetrieveCadencyStartDate(idLead);
                return Ok(cadencyStartDate);
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro na consulta: {ex.Message}");

            }
        } // Completo

        [HttpGet("{idLead}/activity/cadency")]
        public async Task<IActionResult> GetCadencyActivityForDay(int idLead)
        {
            try
            {
                var listCadencyActivityForDay = await _leadService.RetriveCadencyActivityForDay(idLead);
                return Ok(listCadencyActivityForDay);
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro na consulta: {ex.Message}");

            }
        } // Completo


        // ------------------------------------- MÉTODOS POST REFATORADOS E CORRETOS -------------------------------------

        [HttpPost("{idAdminOrLeader}")]
        public async Task<IActionResult> PostLead(int idAdminOrLeader, [FromBody] CreateLeadRequest createLeadRequest)
        {
            try
            {
                LeadResponse leadResponse = await _leadService.RegisterLead(idAdminOrLeader, createLeadRequest);
                return Ok(leadResponse);
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro ao inserir os dados: {ex.Message}");
            }
        } // Completo

        [HttpPost("batch/{idAdminOrLeader}")]
        public async Task<IActionResult> PostLeadsList(int idAdminOrLeader, [FromBody] List<CreateLeadRequest> createLeadRequestList)
        {
            try
            {
                List<LeadResponse> leadResponseList = await _leadService.RegisterLeadsList(idAdminOrLeader, createLeadRequestList);
                return Ok(leadResponseList);
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro ao inserir os dados: {ex.Message}");
            }
        } // Completo

        [HttpPost("{idLead}/portal/{idLoginPortal}")]
        public async Task<IActionResult> PostProspectionActivity(int idLead, int idLoginPortal, [FromBody] LeadRegisterActivity leadRegisterActivity)
        {
            try
            {
                await _leadService.RegisterProspectingActivity(idLead, idLoginPortal, leadRegisterActivity);
                return Ok();
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro ao inserir os dados: {ex.Message}");
            }
        } // Completo

        // ------------------------------------- MÉTODOS PUT REFATORADOS E CORRETOS -------------------------------------

        [HttpPut("{leadId}")]
        public async Task<IActionResult> PutLead(int leadId, [FromBody] UpdateLeadRequest updateLeadRequest)
        {
            try
            {
                var updatedLead = await _leadService.ModifyLead(leadId, updateLeadRequest);
                return Ok(updatedLead);
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro ao inserir os dados: {ex.Message}");
            }
        } // Completo

        [HttpPut("status/{idLead}")]
        public async Task<IActionResult> PutLeadStatus(int idLead, [FromBody] string statusProspeccao)
        {
            try
            {
                await _leadService.ModifyLeadProspectingStatus(idLead, statusProspeccao);
                return Ok();
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro ao inserir os dados: {ex.Message}");
            }
        } // Completo


        // ------------------------------------- MÉTODOS DELETE REFATORADOS E CORRETOS -------------------------------------

        [HttpDelete("{leadId}")] // Funcionando, falta tratar erros e colocar na classe de servico.
        public async Task<IActionResult> DeleteLead(int leadId)
        {
            // Verifica se existe
            var existing = await _supabase
                .From<LeadDbRepresent>()
                .Where(e => e.LeadId == leadId)
                .Get();

            if (!existing.Models.Any())
                return NotFound("Lead não encontrado.");

            await _supabase
                .From<LeadDbRepresent>()
                .Where(e => e.LeadId == leadId)
                .Delete();

            return NoContent(); // 204
        }


        [HttpPost("{idLoginPortal}/cliente")]
        public async Task<IActionResult> PostNewClient(int idLoginPortal, [FromBody] CreateClientRequest createClientRequest)
        {
            try
            {
                await _sdrService.RegisterNewClient(idLoginPortal, createClientRequest);
                return Ok();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }
    }
}
