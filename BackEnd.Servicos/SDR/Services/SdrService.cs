using BackEnd.Modelos.SDR.DTO.DataTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Servicos.SDR.Services
{
    public class SdrService
    {
        private readonly MotoMatsuoSupabaseClient _matsuoSupabaseClient;

        public SdrService(MotoMatsuoSupabaseClient matsuoSupabaseClient)
        {
            _matsuoSupabaseClient = matsuoSupabaseClient;
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
                    GenerateRandomTemporaryId(),
                    "[15D] In Hold"
                );

                //int dadosAxiliarID = 45; VOU deixar mockado com meu ID por enquanto
                string generalDataIdRandom = generalData.id_cliente;
                await _matsuoSupabaseClient.InsertGeneralData(generalData);
                int generalDataId = await _matsuoSupabaseClient.SelectGeneralDataId(generalDataIdRandom);
                await _matsuoSupabaseClient.InsertGeneralTable(new GeneralTableData(DateTime.UtcNow, contactId, generalDataId, idLoginPortal));
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private string GenerateRandomTemporaryId()
        {
            const string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var bytes = Guid.NewGuid().ToByteArray();

            return new string(bytes
                .Take(6)
                .Select(b => letras[b % letras.Length])
                .ToArray());
        } // Gera um ID temporário de 6 caracteres

    }
}
