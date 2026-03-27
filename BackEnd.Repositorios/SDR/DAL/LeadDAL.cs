using BackEnd.Modelos.SDR.DTO.Lead;
using BackEnd.Modelos.SDR.DTO.LogActivity;
using BackEnd.Modelos.SDR.Enums;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Repositorios.SDR.Data_Representations;
using BackEnd.Repositorios.SDR.Exceptions;
using Supabase;

namespace BackEnd.Repositorios.SDR.DAL
{
    public class LeadDAL
    {
        private Client _supabase;

        public LeadDAL(Client supabase)
        {
            _supabase = supabase;
        }

        // -------------------------------- MÉTODOS PÚBLICOS REFATORADOS E COMPLETOS --------------------------------

        public async Task<IEnumerable<SimpleLeadResponse>> SelectLeads()
        {
            var response = await _supabase
                .From<LeadDbRepresent>()
                .Order(l => l.LeadId, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            var simpleLeadResponse = response.Models.Select(e => new SimpleLeadResponse
            (
                e.LeadId,
                e.RazaoSocial,
                e.NomeFantasia,
                e.CNPJ,
                e.Setor,
                e.Faturamento,
                e.Site,
                e.DataInclusao.ToShortDateString(),
                ((ProspectionStatus)e.StatusProspeccaoFk).ToString(),
                e.LoginPortalFk,
                e.IdProtheus,
                e.IdVendedor
            )).ToList();

            return simpleLeadResponse;
        } // Completo

        public async Task<Lead> SelectSpecificLead(int idLead)
        {
            var leadDb = await _supabase
                 .From<LeadDbRepresent>()
                 .Where(l => l.LeadId == idLead)
                 .Get();

            var leadDbTest = leadDb.Models.FirstOrDefault();

            if (leadDbTest is null)
                throw new RepositoriesException("Não foi encontrado nenhum lead com o ID informado."); // caiu nesse erro quando eu procurei um id q não exite

            var addressDb = await _supabase
                .From<AddressDbRepresent>()
                .Where(e => e.LeadFk == idLead)
                .Get();

            var contatoDb = await _supabase
                .From<ContactDbRepresent>()
                .Where(c => c.LeadFk == idLead)
                .Get();

            List<LeadAddress> Addresses = new List<LeadAddress>();

            if (!(addressDb.Models is null || addressDb.Models.Count == 0))
            {
                Addresses = addressDb.Models
                    .Select(e => new LeadAddress(e.Rua, e.Numero, e.Bairro, e.Cidade, e.UF))
                    .ToList();
            }

            List<LeadContact> contatos = new List<LeadContact>();

            if (!(contatoDb.Models is null || contatoDb.Models.Count == 0))
            {
                foreach (var c in contatoDb.Models)
                {
                    var numeroDbResponse = await _supabase
                        .From<NumberDbRepresent>()
                        .Where(n => n.ContatoFk == c.ContatoId)
                        .Get();

                    var numeros = numeroDbResponse.Models
                        .Select(n => new LeadNumber(n.Numero, n.Tipo, n.Whatsapp))
                        .ToList();

                    contatos.Add(new LeadContact(c.Nome, c.Cargo, c.Email) { LeadNumbers = numeros });

                }
            }

            var lead = new Lead(leadDbTest.LeadId, leadDbTest.RazaoSocial, leadDbTest.NomeFantasia, leadDbTest.CNPJ, leadDbTest.Setor, leadDbTest.Faturamento,
                leadDbTest.Site, leadDbTest.DataInclusao, (ProspectionStatus)leadDbTest.StatusProspeccaoFk)
            { IdProtheus = leadDbTest.IdProtheus, IdVendedor = leadDbTest.IdVendedor, EnderecosLead = Addresses, ContatosLead = contatos };

            return lead;
        } // Completo

        public async Task<DateTime> SelectCadencyStartDate(int idLead)
        {

            var responseLead = await _supabase
                .From<LeadDbRepresent>()
                .Where(l => l.LeadId == idLead)
                .Select("cnpj")
                .Get();

            string cnpj = responseLead.Models.First().CNPJ;
            string description = $"Status do lead atualizado | CNPJ: {cnpj} | Status atual: TentandoContato";

            var response = await _supabase
                .From<ActivityLogDbRepresent>()
                .Where(al => al.LeadFk == idLead)
                .Where(al => al.Description == description)
                .Where(al => al.StatusProspeccaoFk == 3)
                .Get();

            var activity = response.Models.FirstOrDefault();
            if (activity is null)
                throw new RepositoriesException("Atividade de início de cadência não encontrada para o lead especificado.");

            return activity.Register;
        } // Completo

        public async Task<List<ActivityForDayResponse>> SelectCadencyActivityForDay(int leadId)
        {
            var activitiesForDay = new List<ActivityForDayResponse>();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var response = await _supabase
                .From<ActivityLogDbRepresent>()
                .Where(l => l.LeadFk == leadId)
                .Get();

            if (response.Models is null || response.Models.Count == 0)
                return activitiesForDay;

            var descriptionsToday = response.Models
                .Where(a => a.Register >= today && a.Register < tomorrow)
                .Select(a => a.Description)
                .ToHashSet();

            if (descriptionsToday.Contains("Atividade de prospecção realizada | Atividade: Email encaminhado"))
                activitiesForDay.Add(new ActivityForDayResponse("Email encaminhado", true));

            if (descriptionsToday.Contains("Atividade de prospecção realizada | Atividade: Ligação realizada"))
                activitiesForDay.Add(new ActivityForDayResponse("Ligação realizada", true));

            if (descriptionsToday.Contains("Atividade de prospecção realizada | Atividade: Mensagem enviada via whatsApp"))
                activitiesForDay.Add(new ActivityForDayResponse("Mensagem enviada via whatsApp", true));

            return activitiesForDay;
        } // Completo

        public async Task<Lead> InsertLead(int idAdminOrLeader, Lead lead, int loginPortalFk)
        {
            int idLead = await this.CreateLead(idAdminOrLeader, lead, loginPortalFk);
            await InsertAddress(lead.EnderecosLead, idLead);
            await InsertContact(lead.ContatosLead, idLead);
            lead.LeadId = idLead;
            return lead;
        } // Completo

        public async Task InsertProspectionActivity(int idLead, int loginPortalFk, int IdProspectionStatus, string description)
        {
            string activityDescription = $"Atividade de prospecção realizada | Atividade: {description}";
            loginPortalFk = 1; // Por enquando vou atribuir 1 pois é o id do usuario de teste que estou utilizando, depois quando for para produção eu removo essa linha e ja vai funcionar corretamente.

            var logDb = new ActivityLogDbRepresent
            {
                Register = DateTime.Now,
                Description = activityDescription,
                StatusProspeccaoFk = IdProspectionStatus,
                LeadFk = idLead,
                // LoginPortalFk = loginPortalFk POR ENQUANTO VOU DEIXAR MOCADO, DEPOIS É SÓ REMOVER ESSA LINHA E DESCOMENTAR ESSA LINHA PARA PEGAR O ID DO USUÁRIO LOGADO 
                LoginPortalFk = loginPortalFk
            };

            var insertResponse = await _supabase
                .From<ActivityLogDbRepresent>()
                .Insert(logDb);
        } // Completo

        public async Task<Lead> UpdateLead(int leadId, Lead lead)
        {
            // Opcional: verificar se existe antes
            var existing = await _supabase
                .From<LeadDbRepresent>()
                .Where(e => e.LeadId == leadId)
                .Get();

            if (!existing.Models.Any())
                throw new RepositoriesException("O lead informado não existe.");

            var leadDb = new LeadDbRepresent
            {
                NomeFantasia = lead.NomeFantasia,
                Setor = lead.Setor,
                Faturamento = lead.Faturamento,
                Site = lead.Site,
            };

            var response = await _supabase
                .From<LeadDbRepresent>()
                .Where(e => e.LeadId == leadId)
                .Set(e => e.NomeFantasia, leadDb.NomeFantasia)
                .Set(e => e.Setor, leadDb.Setor)
                .Set(e => e.Faturamento, leadDb.Faturamento)
                .Set(e => e.Site, leadDb.Site)
                .Update();

            var updated = response.Models.FirstOrDefault();

            if (updated == null)
                throw new RepositoriesException("Ocorreu um erro ao atualizar Lead.");

            var updatedLead = new Lead(updated.NomeFantasia, updated.Setor, updated.Faturamento, updated.Site);

            return updatedLead;
        } // Completo

        public async Task UpdateProspectingStatus(int idLead, int statusProspecao)
        {
            var response = await _supabase
                .From<LeadDbRepresent>()
                .Where(l => l.LeadId == idLead)
                .Set(l => l.StatusProspeccaoFk, statusProspecao)
                .Update();

            if (response.Models == null || !response.Models.Any())
                throw new RepositoriesException("Lead não encontrado ou atualização falhou.");

            await RegisterActivityToChangeStatus(idLead, statusProspecao);
        } // Completo


        // -------------------------------- MÉTODOS PRIVADOS REFATORADOS E COMPLETOS --------------------------------

        private async Task<int> CreateLead(int idAdminOrLeader, Lead lead, int loginPortalFk)
        {
            var leadDb = new LeadDbRepresent
            {
                RazaoSocial = lead.RazaoSocial,
                NomeFantasia = lead.NomeFantasia,
                CNPJ = lead.CNPJ,
                Setor = lead.Setor,
                Faturamento = lead.Faturamento,
                Site = (lead.Site != "") ? lead.Site : null, // coloquei essa condição para ver se passa
                DataInclusao = DateTime.Now,
                StatusProspeccaoFk = 1,
                LoginPortalFk = loginPortalFk,
            };

            var response = await _supabase
                .From<LeadDbRepresent>()
                .Insert(leadDb);

            var created = response.Models.FirstOrDefault();

            if (created == null)
                throw new RepositoriesException("Erro ao inserir lead.");

            await this.RegisterFirstActivity(created.LeadId, idAdminOrLeader);

            return created.LeadId;
        } // Completo

        private async Task InsertAddress(List<LeadAddress> addresses, int idLead)
        {
            if (addresses == null || addresses.Count == 0)
                return;
            List<AddressDbRepresent> addressList = addresses
                .Select(el => new AddressDbRepresent { Rua = el.Street, Numero = el.Number, Bairro = el.Neighborhood, Cidade = el.City, UF = el.UF, LeadFk = idLead })
                .ToList();

            var response = await _supabase
            .From<AddressDbRepresent>()
            .Insert(addressList);

            var created = response.Models.FirstOrDefault();

            if (created == null)
                throw new RepositoriesException("Erro ao inserir endereço.");
        } // Completo

        private async Task InsertContact(List<LeadContact> contatos, int idLead)
        {
            if (contatos == null || contatos.Count == 0)
                return;

            List<ContactDbRepresent> listaContatos = new List<ContactDbRepresent>();
            foreach (LeadContact ct in contatos)
            {
                var contatoDb = new ContactDbRepresent
                {
                    Nome = ct.Name,
                    Cargo = ct.JobTitle,
                    Email = (ct.Email != "")? ct.Email : null, //colocoquei essa condição para ver se passa
                    LeadFk = idLead
                };

                var response = await _supabase
                    .From<ContactDbRepresent>()
                    .Insert(contatoDb);

                var created = response.Models.FirstOrDefault();

                if (created == null)
                    throw new RepositoriesException("Erro ao inserir contato.");

                int idContato = created.ContatoId;

                if (ct.LeadNumbers != null && ct.LeadNumbers.Count != 0)
                    await this.InsertNumber(ct.LeadNumbers, idContato);
            }
        } // Completo

        private async Task InsertNumber(List<LeadNumber> numeros, int idContato)
        {
            List<NumberDbRepresent> listaNumeros = numeros
                .Select(nr => new NumberDbRepresent { Numero = nr.Number, Tipo = nr.Type, Whatsapp = nr.Whatsapp, ContatoFk = idContato })
                .ToList();

            var response = await _supabase
                .From<NumberDbRepresent>()
                .Insert(listaNumeros);

            var created = response.Models.FirstOrDefault();

            if (created == null)
                throw new RepositoriesException("Erro ao criar numero.");
        } // Completo

        private async Task RegisterActivityToChangeStatus(int idLead, int statusProspecao)
        {
            var selectResponse = await _supabase
                .From<LeadDbRepresent>()
                .Where(l => l.LeadId == idLead)
                .Get();

            string cnpj = selectResponse.Models.FirstOrDefault().CNPJ;
            int idLoginPortal = selectResponse.Models.FirstOrDefault().LoginPortalFk;

            string description = $"Status do lead atualizado | CNPJ: {cnpj} | Status atual: {(ProspectionStatus)statusProspecao}";

            var logDb = new ActivityLogDbRepresent
            {
                LeadFk = idLead,
                Description = description,
                Register = DateTime.Now,
                //LoginPortalFk = 1, // Por enquando vou deixar mocado, depois a gente vê como pegar o id do usuário logado
                LoginPortalFk = idLoginPortal,
                StatusProspeccaoFk = (int)((ProspectionStatus)statusProspecao)
            };

            var insertResponse = await _supabase
                .From<ActivityLogDbRepresent>()
                .Insert(logDb);
        } // Completo


        // -------------------------------- MÉTODOS PARA REFATORAÇÂO --------------------------------




        private async Task RegisterFirstActivity(int idLead, int idAdminOrLeader)
        {
            var response = await _supabase
                .From<LeadDbRepresent>()
                .Select("cnpj")
                .Where(l => l.LeadId == idLead)
                .Get();

            var cnpj = response.Models.FirstOrDefault()?.CNPJ;

            // Inclusão de lead realizada | CNPJ: 76908266889527 | Status inicial: Qualificacao
            string description = $"Inclusão de lead realizada | CNPJ: {cnpj} | Status inicial: {ProspectionStatus.NovoLead}";

            var logDb = new ActivityLogDbRepresent
            {
                LeadFk = idLead,
                Description = description,
                Register = DateTime.Now,
                LoginPortalFk = idAdminOrLeader,
                //LoginPortalFk = 1,
                StatusProspeccaoFk = (int)ProspectionStatus.NovoLead
            };

            var insertResponse = await _supabase
                .From<ActivityLogDbRepresent>()
                .Insert(logDb);

            if (insertResponse.Models == null)
                throw new RepositoriesException("Erro ao registrar atividade de inclusão de lead.");
        }
    }
}
