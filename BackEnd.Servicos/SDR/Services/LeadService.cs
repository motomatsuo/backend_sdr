using BackEnd.Modelos.SDR.DTO.Address;
using BackEnd.Modelos.SDR.DTO.Contact;
using BackEnd.Modelos.SDR.DTO.Lead;
using BackEnd.Modelos.SDR.DTO.LogActivity;
using BackEnd.Modelos.SDR.DTO.LoginPortal;
using BackEnd.Modelos.SDR.DTO.Number;
using BackEnd.Modelos.SDR.Exceptions;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Repositorios.SDR.DAL;
using BackEnd.Repositorios.SDR.Exceptions;
using BackEnd.Servicos.SDR.Interfaces;
using BackEnd.Servicos.SDR.Validacoes;
using System.Linq;


namespace BackEnd.Servicos.SDR.Services
{
    public class LeadService
    {
        private Lead _lead;
        private List<Lead> _leads = new List<Lead>();
        private readonly ILeadValidator _leadValidator;
        private readonly LeadDAL _leadDAL;
        private readonly LoginPortalService _loginPortalService;


        public LeadService(LeadDAL leadDAL, ILeadValidator leadValidator, LoginPortalService loginPortalService)
        {
            _leadDAL = leadDAL;
            _leadValidator = leadValidator;
            _loginPortalService = loginPortalService;
        }


        // -------------------------------- MÉTODOS PÚBLICOS REFATORADOS E COMPLETOS --------------------------------

        public async Task<HashSet<SimpleLeadResponse>> RetrieveAllLeads()
        {
            var results = await _leadDAL.SelectLeads();
            HashSet<SimpleLeadResponse> leadsResponse = leadsResponse = results.ToHashSet();
            return leadsResponse;
        } // Completo

        public async Task<LeadResponse> RetrieveSpecificLeadById(int idLead)
        {
            try
            {
                _leadValidator.ValidateLeadId(idLead);
                _lead = await _leadDAL.SelectSpecificLead(idLead);
                LeadResponse lead = ConvertLeadToResponse(_lead);
                return lead;
            }
            catch (RepositoriesException ex)
            {
                throw new RepositoriesException(ex.Message); // caiu nesse erro quando eu procurei um id q não exite
            }
            catch (ModelException ex)
            {
                throw new ModelException(ex.Message); // caiu nesse erro quando eu coloquei um id negativo
            }

        } // Completo

        public async Task<DateTime> RetrieveCadencyStartDate(int idLead)
        {
            try
            {
                _leadValidator.ValidateLeadId(idLead);
                return await _leadDAL.SelectCadencyStartDate(idLead);
            }
            catch (ModelException ex)
            {
                throw new ModelException("Foi atribuido um valor inválido para o código do lead"); // Deu erro quando eu passei 0 como argumento na requisição

            }
            catch (RepositoriesException ex)
            {
                throw new RepositoriesException("Atividade de início de cadência não encontrada para o lead especificado.");
            }
        } // Completo

        public async Task<List<ActivityForDayResponse>> RetriveCadencyActivityForDay(int leadId)
        {
            try
            {
                _leadValidator.ValidateLeadId(leadId);
                return await _leadDAL.SelectCadencyActivityForDay(leadId);
            }
            catch (ModelException ex)
            {
                throw new ModelException("Foi atribuido um valor inválido para o código do lead"); // Deu erro quando eu passei 0 como argumento na requisição

            }
            catch (RepositoriesException ex)
            {
                throw new RepositoriesException("Atividade de início de cadência não encontrada para o lead especificado.");
            }
        } // Completo

        public async Task<LeadResponse> RegisterLead(CreateLeadRequest leadRequest)
        {
            try
            {
                _lead = ConvertRequestToLead(leadRequest); // Tem que ser ao contrario, 1º Converte 2º valida
                _leadValidator.ValidateLead(_lead);
                var createdLead = await _leadDAL.InsertLead(_lead, 1); // Coloquei 1 como mock só para teste
                return ConvertLeadToResponse(createdLead);
            }
            catch (ModelException ex)
            {
                throw new ModelException(ex.Message);
            }
            catch (RepositoriesException ex)
            {
                throw new RepositoriesException(ex.Message);
            }
        } // Completo

        public async Task<List<LeadResponse>> RegisterLeadsList(List<CreateLeadRequest> leadRequestList)
        {
            try
            {
                if (leadRequestList == null || !leadRequestList.Any())
                    throw new ModelException("A lista de leads está vazia.");

                /*
                var usersWithLeads = await _loginPortalService.RetriveLeadsUsersAsync();
                var userIds = usersWithLeads
                    .Select(u => u.IdLoginPortal)
                    .ToList();

                if (!userIds.Any())
                    throw new ModelException("Nenhum usuário disponível para receber leads.");
                */

                List<int> userIds = new List<int> {1, 2, 3 }; // Mock para teste

                var leadsCriados = new List<Lead>();
                int index = 0;

                foreach (var createLeadRequest in leadRequestList)
                {
                    var userId = userIds[index];

                    var lead = ConvertRequestToLead(createLeadRequest);

                    _leadValidator.ValidateLead(lead);

                    var leadCriado = await _leadDAL.InsertLead(lead, userId);
                    leadsCriados.Add(leadCriado);

                    index = (index + 1) % userIds.Count;
                }

                return leadsCriados
                    .Select(ConvertLeadToResponse)
                    .ToList();
            }
            catch (ModelException ex)
            {
                throw new ModelException(ex.Message);
            }
            catch (RepositoriesException ex)
            {
                throw new RepositoriesException(ex.Message);
            }
        } // Completo

        public async Task RegisterProspectingActivity(int idLead, int loginPortalFk, LeadRegisterActivity leadRegisterActivity)
        {
            try
            {
                _leadValidator.ValidateLeadId(idLead);
                _leadValidator.ValidateIdLoginPortal(loginPortalFk);
                _leadValidator.ValidateDescription(leadRegisterActivity.Description);
                _leadValidator.ValidateProspectingStatus(leadRegisterActivity.StatusProspeccao);
                int idProspectionStatus = _leadValidator.ConvertProspectingStatusToInt(leadRegisterActivity.StatusProspeccao);
                await _leadDAL.InsertProspectionActivity(idLead, loginPortalFk, idProspectionStatus, leadRegisterActivity.Description);
            }
            catch (ModelException ex)
            {
                throw new ModelException(ex.Message);
            }
            catch (RepositoriesException ex)
            {
                throw new RepositoriesException(ex.Message);
            }

        } // Completo

        public async Task<UpdatedLeadResponse> ModifyLead(int idLead, UpdateLeadRequest updateLead)
        {
            try
            {
                _leadValidator.ValidateLeadId(idLead);
                _leadValidator.ValidateUpdateLead(updateLead);
                Lead leadToUpdate = ConvertUpdateLeadRequestToLead(updateLead);
                leadToUpdate = await _leadDAL.UpdateLead(idLead, leadToUpdate);
                return ConvertLeadToUpdatedLead(leadToUpdate);
            }
            catch (ModelException ex)
            {
                throw new ModelException(ex.Message);
            }
            catch (RepositoriesException ex)
            {
                throw new RepositoriesException(ex.Message);
            }
        } // Completo

        public async Task ModifyLeadProspectingStatus(int idLead, string prospectingStatus)
        {
            _leadValidator.ValidateLeadId(idLead);
            _leadValidator.ValidateProspectingStatus(prospectingStatus);
            int idProspectingStatus = _leadValidator.ConvertProspectingStatusToInt(prospectingStatus);
            await _leadDAL.UpdateProspectingStatus(idLead, idProspectingStatus);
        } // Completo


        // -------------------------------- MÉTODOS PRIVADOS REFATORADOS E COMPLETOS --------------------------------

        private LeadResponse ConvertLeadToResponse(Lead lead)
        {
            LeadResponse leadResponse = new LeadResponse
            (
                lead.LeadId,
                lead.RazaoSocial,
                lead.NomeFantasia,
                lead.CNPJ,
                lead.Setor,
                lead.Faturamento,
                lead.Site,
                lead.DataInclusao,
                lead.StatusProspeccao.ToString(),
                lead.EnderecosLead
                    .Select(e => new SimpleAddressResponse(e.Street, e.Number, e.Neighborhood, e.City, e.UF))
                    .ToList(),
                lead.ContatosLead
                    .Select(c => new SimpleContactResponse(c.Name, c.JobTitle, c.Email, c.LeadNumbers
                    .Select(n => new SimpleNumberResponse(n.Type, n.Number, n.Whatsapp))))
                    .ToList()
            );
            return leadResponse;
        } // Completo

        private Lead ConvertRequestToLead(CreateLeadRequest request)
        {
            var lead = new Lead(
                request.RazaoSocial,
                request.NomeFantasia,
                request.CNPJ,
                request.Setor,
                request.Faturamento,
                request.Site
            );

            AddAddresses(lead, request);
            AddContacts(lead, request);

            return lead;
        } // Completo

        private void AddAddresses(Lead lead, CreateLeadRequest request)
        {
            if (request.Enderecos == null) return;

            foreach (var endereco in request.Enderecos)
            {
                lead.EnderecosLead.Add(new LeadAddress(
                    endereco.Rua,
                    endereco.Numero,
                    endereco.Bairro,
                    endereco.Cidade,
                    endereco.Uf
                ));
            }
        } // Completo

        private void AddContacts(Lead lead, CreateLeadRequest request)
        {
            if (request.Contatos == null) return;

            foreach (var contatoRequest in request.Contatos)
            {
                var contato = new LeadContact(
                    contatoRequest.Nome,
                    contatoRequest.Cargo,
                    contatoRequest.Email
                );

                AddNumbers(contato, contatoRequest);

                lead.ContatosLead.Add(contato);
            }
        } // Completo

        private void AddNumbers(LeadContact contato, CreateContactsRequest request)
        {
            if (request.Numeros == null) return;

            foreach (var numero in request.Numeros)
            {
                contato.LeadNumbers.Add(new LeadNumber(
                    numero.Numero,
                    numero.Tipo,
                    numero.Whatsapp
                ));
            }
        } // Completo   

        private Lead ConvertUpdateLeadRequestToLead(UpdateLeadRequest updateLead)
        {
            return new Lead(updateLead.NomeFantasia, updateLead.Setor, updateLead.Faturamento, updateLead.Site);
        } // Completo

        private UpdatedLeadResponse ConvertLeadToUpdatedLead(Lead leadToUpdate)
        {
            return new UpdatedLeadResponse(leadToUpdate.NomeFantasia, leadToUpdate.Setor, leadToUpdate.Faturamento, leadToUpdate.Site);
        } // Completo


        // -------------------------------- MÉTODOS PARA REFATORAÇÂO --------------------------------

    }
}
