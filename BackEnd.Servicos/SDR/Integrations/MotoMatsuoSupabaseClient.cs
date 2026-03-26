using BackEnd.Modelos.SDR.DTO.DataTables;
using BackEnd.Modelos.SDR.DTO.LoginPortal;
using BackEnd.Modelos.SDR.Modelos;
using System.Net.Http.Json;


public class MotoMatsuoSupabaseClient
{
    private readonly HttpClient _httpClient;

    public MotoMatsuoSupabaseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LoginPortal>> SelectLeadUsersAsync()
    {
        // preciso adicionar o tratamento de erros caso ocorra erro com a conexão com o cliente.
        using var response = await _httpClient.GetAsync("rest/v1/db_login_portal?select=id,nome,grupo");

        response.EnsureSuccessStatusCode();

        var list = await response.Content.ReadFromJsonAsync<List<LoginPortal>>() ?? new List<LoginPortal>();
        return list;
    }

    public async Task InsertDataContact(ContactsDataRequest contactsData)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync("rest/v1/db_dadoscontato_portal_sdr", contactsData);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro Supabase, Cliente já existente na base: {error}"); // deu exceção aqui
            }
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    } // Completo

    public async Task InsertComplementaryData(ComplementaryDataRequest complementaryData)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync("rest/v1/db_dadoscomplementares_portal_sdr", complementaryData);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro Supabase: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    } // Completo

    public async Task InsertGeneralData(GeneralDataRequest generalData) {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync("rest/v1/db_dadosgerais_portal_sdr", generalData);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro Supabase: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    } // Completo

    public async Task InsertGeneralTable(GeneralTableData generalTable)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync("rest/v1/db_tabelageral_portal_sdr", generalTable);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro Supabase: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    }

    public async Task<int> SelectContactIdForCNPJ(ContactsDataRequest contactsData)
    {
        try
        {
            string cnpj = contactsData.cnpj;
            using var response = await _httpClient.GetAsync($"rest/v1/db_dadoscontato_portal_sdr?cnpj=eq.{cnpj}&limit=1&select=id");

            response.EnsureSuccessStatusCode();
            var contactList = await response.Content.ReadFromJsonAsync<List<ContactIdResponse>>() ?? new List<ContactIdResponse>();

            if (contactList.Count == 0)
                throw new Exception("Nenhum contato encontrado para o CNPJ fornecido.");

            return contactList[0].Id; // Retorna o ID do primeiro contato encontrado
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    } // Completo

    public async Task<int> SelectGeneralDataId(string clientRandomId)
    {
        try
        {
            using var response = await _httpClient.GetAsync($"rest/v1/db_dadosgerais_portal_sdr?id_cliente=eq.{clientRandomId}&limit=1&select=id");

            response.EnsureSuccessStatusCode();
            var contactList = await response.Content.ReadFromJsonAsync<List<ContactIdResponse>>() ?? new List<ContactIdResponse>();

            if (contactList.Count == 0)
                throw new Exception("Nenhum contato encontrado para o ID fornecido.");

            return contactList[0].Id; // Retorna o ID do primeiro contato encontrado
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    }

    public async Task UpdateLoginPortalGroupAsync(int loginPortalId, UpdatedGroup updatedGroup)
    {
        using var response = await _httpClient.PatchAsync($"rest/v1/db_login_portal?id=eq.{loginPortalId}", JsonContent.Create(updatedGroup));
        response.EnsureSuccessStatusCode();
    }

    public async Task<LoginPortal> SelectOneEspecificLoginPortalGroupAsync(int loginPortalId)
    {
        using var response = await _httpClient.GetAsync($"rest/v1/db_login_portal?select=id,nome,grupo&id=eq.{loginPortalId}");
        response.EnsureSuccessStatusCode();
        var list = await response.Content.ReadFromJsonAsync<List<LoginPortal>>() ?? new List<LoginPortal>();

        return list[0];
        //using var updatedResponse = await _httpClient.PutAsJsonAsync("rest/v1/db_login_portal?some_column=eq.someValue"); // verificar se esta certo
        //updatedResponse.EnsureSuccessStatusCode();
    }

    public async Task<List<SdrUsersResponse>> SelectSdrUsersAsync()
    {
        // preciso adicionar o tratamento de erros caso ocorra erro com a conexão com o cliente.
        using var response = await _httpClient.GetAsync("rest/v1/db_login_portal?select=id,nome,grupo,funcao");

        response.EnsureSuccessStatusCode();

        var list = await response.Content.ReadFromJsonAsync<List<SdrUsersResponse>>() ?? new List<SdrUsersResponse>();
        return list;
    }

    public async Task<List<SellersResponse>> SelectSellersAsync()
    {
        try
        {
            using var response = await _httpClient.GetAsync("rest/v1/db_vendedores?select=id_protheus,nome");

            response.EnsureSuccessStatusCode();

            var list = await response.Content.ReadFromJsonAsync<List<SellersResponse>>() ?? new List<SellersResponse>();
            return list;
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    } // Completo

    public async Task InsertIdProtheus(int leadId, IdProtheusRequest idProtheus)
    {
        try
        {
            using var response = await _httpClient.PatchAsJsonAsync($"rest/v1/leads?lead_id=eq.{leadId}", idProtheus);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro Supabase: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    } // Completo

    public async Task InsertIdVendedor(int leadId, IdVendedorRequest idVendedor)
    {
        try
        {
            using var response = await _httpClient.PatchAsJsonAsync($"rest/v1/leads?lead_id=eq.{leadId}", idVendedor);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro Supabase: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
        }
    } // Completo

}