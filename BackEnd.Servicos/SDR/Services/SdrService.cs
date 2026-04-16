using BackEnd.Modelos.SDR.DTO.DataTables;
using BackEnd.Modelos.SDR.DTO.Lead;
using BackEnd.Repositorios.SDR.Data_Representations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Servicos.SDR.Services
{
    public class SdrService
    {
        private readonly MotoMatsuoSupabaseClient _matsuoSupabaseClient;
        private readonly LeadService _leadService;
        public SdrService(MotoMatsuoSupabaseClient matsuoSupabaseClient, LeadService leadService)
        {
            _matsuoSupabaseClient = matsuoSupabaseClient;
            _leadService = leadService;
        }

        public async Task RegisterNewClient(int idLoginPortal, CreateClientRequest createClientRequest)
        {
            try
            {
                ContactsDataRequest contactsData = createClientRequest.ContactsData;
                await _matsuoSupabaseClient.InsertDataContact(contactsData);
                int contactId = await _matsuoSupabaseClient.SelectContactIdForCNPJ(contactsData);
                //new ComplementaryDataRequest();
                ComplementaryDataRequest complementaryData = new ComplementaryDataRequest
                (
                    createClientRequest.ComplementaryData.created_at,
                    contactId,
                    createClientRequest.ComplementaryData.comercializa_motos,
                    createClientRequest.ComplementaryData.tipo_moto,
                    createClientRequest.ComplementaryData.comercializa_peca,
                    createClientRequest.ComplementaryData.tipo_loja_peca,
                    createClientRequest.ComplementaryData.prestacao_servico,
                    createClientRequest.ComplementaryData.link_loja_online,
                    createClientRequest.ComplementaryData.tipo_prestacao_servico,
                    createClientRequest.ComplementaryData.como_conheceu,
                    createClientRequest.ComplementaryData.vendedor_indicado,
                    createClientRequest.ComplementaryData.modelo_empresa
                );
                await _matsuoSupabaseClient.InsertComplementaryData(complementaryData);

                GeneralDataRequest generalData = new GeneralDataRequest
                (
                    "Prosp-Ativa",
                    "Novo Cadastro",
                    "Lista-Lead",
                    DateTime.UtcNow,
                    createClientRequest.clientId,
                    "[15D] In Hold"
                );

                //int dadosAxiliarID = 45; VOU deixar mockado com meu ID por enquanto
                string generalDataIdRandom = generalData.id_cliente;
                await _matsuoSupabaseClient.InsertGeneralData(generalData);
                int generalDataId = await _matsuoSupabaseClient.SelectGeneralDataId(generalDataIdRandom);

                // Preciso chamar a função de registrar datas aqui 
                var ids = await _leadService.RegisterMonitoringDate(createClientRequest.ComplementaryData.created_at);



                await _matsuoSupabaseClient.InsertGeneralTable(new GeneralTableData(DateTime.UtcNow, contactId, generalDataId, ids[0], ids[1], ids[2], idLoginPortal));
                int contactIdClient = await _matsuoSupabaseClient.SelectContactIdClientAsync(createClientRequest.ContactsData.cnpj);
                await _matsuoSupabaseClient.UpdateGeneralTableAsync(contactIdClient, createClientRequest.DadosVendedorRequest.dados_vendedor);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        } // Completo

        private string GenerateRandomTemporaryId()
        {
            const string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var bytes = Guid.NewGuid().ToByteArray();

            return new string(bytes
                .Take(6)
                .Select(b => letras[b % letras.Length])
                .ToArray());
        } // Gera um ID temporário de 6 caracteres


        /* Versão para testar
         * 
        public async Task<int> InsertFirstMonitoringReq(PostMonitoringReq monitoring)
        {
            try
            {
                var insert = new FirstMonitoringDbRepresent
                {
                    DataAcompanhamento = monitoring.MontoringDate,
                };

                var dbInsertResponse = await _supabase
                    .From<FirstMonitoringDbRepresent>()
                    .Insert(insert);

                if (dbInsertResponse == null)
                    throw new Exception($"Erro Supabase, Falha no teste"); // deu exceção aqui

                var id = dbInsertResponse.Models.FirstOrDefault().Id;
                Console.WriteLine(id);
                return id;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
            }
        }

        public async Task<int> InsertSecondtMonitoringReq(PostMonitoringReq monitoring)
        {
            try
            {
                var insert = new SecondMonitoringDbRepresent
                {
                    DataAcompanhamento = monitoring.MontoringDate,
                };

                var dbInsertResponse = await _supabase
                    .From<SecondMonitoringDbRepresent>()
                    .Insert(insert);

                if (dbInsertResponse == null)
                    throw new Exception($"Erro Supabase, Falha no teste"); // deu exceção aqui

                var id = dbInsertResponse.Models.FirstOrDefault().Id;
                Console.WriteLine(id);
                return id;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
            }
        }

        public async Task<int> InsertThirthMonitoringReq(PostMonitoringReq monitoring)
        {
            try
            {
                var insert = new ThirdMonitoringDbRepresent
                {
                    DataAcompanhamento = monitoring.MontoringDate,
                };

                var dbInsertResponse = await _supabase
                    .From<ThirdMonitoringDbRepresent>()
                    .Insert(insert);

                if (dbInsertResponse == null)
                    throw new Exception($"Erro Supabase, Falha no teste"); // deu exceção aqui

                var id = dbInsertResponse.Models.FirstOrDefault().Id;
                Console.WriteLine(id);
                return id;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Erro ao comunicar com o Supabase.", ex);
            }
        }
        */
    }
}
