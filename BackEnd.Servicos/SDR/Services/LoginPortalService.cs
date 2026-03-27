using BackEnd.Modelos.SDR.DTO.LoginPortal;
using BackEnd.Modelos.SDR.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Servicos.SDR.Services
{
    public class LoginPortalService
    {
        private readonly MotoMatsuoSupabaseClient _matsuoSupabaseClient;

        public LoginPortalService(MotoMatsuoSupabaseClient matsuoSupabaseClient)
        {
            _matsuoSupabaseClient = matsuoSupabaseClient;
        }

        public async Task<List<LoginResponse>> RetriveLeadsUsersAsync()
        {
            var sdrUsers = await _matsuoSupabaseClient.SelectLeadUsersAsync();

            // Filtrar os usuários que pertencem ao grupo "LEADS"
            return sdrUsers.Where(user => user.Grupo != null && user.Grupo.Contains("LEADS")).Select(user => new LoginResponse(user.Id, user.Nome)).ToList();
        }

        public async Task ModifyLoginPortalGroupAsync(int loginPortalId)
        {
            var actualRegister = await _matsuoSupabaseClient.SelectOneEspecificLoginPortalGroupAsync(loginPortalId);

            actualRegister.Grupo ??= new List<string>();

            if (!actualRegister.Grupo.Contains("LEADS"))
            {
                actualRegister.Grupo.Add("LEADS");
                // Se não tiver a atribuição LEADS ele adiciona
            }
            else
            {
                // se tiver a atribuição LEADS ele remove a atribuição 
                actualRegister.Grupo.Remove("LEADS");
            }
            var request = new UpdatedGroup(actualRegister.Grupo);
            await _matsuoSupabaseClient.UpdateLoginPortalGroupAsync(loginPortalId, request);
        }

        public async Task<List<LoginResponse>> RetriveSdrUsersAsync()
        {
            var sdrUsers = await _matsuoSupabaseClient.SelectSdrUsersAsync();

            // Filtrar os usuários que pertencem ao grupo "LEADS"
            return sdrUsers.Where(user => user.Grupo != null && user.Grupo.Contains("SDR") && user.Funcao == "User" && !(user.Grupo.Contains("LEADS"))).Select(user => new LoginResponse(user.Id, user.Nome)).ToList();
        }

        public async Task<List<SellersResponse>> RetrieveSellersAsync()
        {
            try
            {
                var sellers = await _matsuoSupabaseClient.SelectSellersAsync();
                sellers.RemoveAll(s => s.Id_Protheus == "4DM1N1");
                sellers.RemoveAll(s => s.Id_Protheus == "4DM1N2");
                return sellers;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        } // Completo

        public async Task RegisterIdProtheusAsync(int leadId, IdProtheusRequest idProtheus)
        {
            await _matsuoSupabaseClient.InsertIdProtheus(leadId, idProtheus);
        }

        public async Task RegisterIdVendedorAsync(int leadId, IdVendedorRequest idVendedor)
        {
            await _matsuoSupabaseClient.InsertIdVendedor(leadId, idVendedor);
        }

        public async Task<int> RetrieveContactIdAsync(string cnpj)
        {
            var contactId = await _matsuoSupabaseClient.SelectContactIdClientAsync(cnpj);

            // Filtrar os usuários que pertencem ao grupo "LEADS"
            return contactId;
        }
    }
}
