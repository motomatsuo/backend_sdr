using BackEnd.Modelos.SDR.DTO;
using BackEnd.Modelos.SDR.DTO.Contact;
using BackEnd.Modelos.SDR.DTO.Number;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Repositorios.SDR.Exceptions;
using Supabase;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Repositorios.SDR.DAL
{
    public class ContactDAL
    {
        private Client _supabase;

        public ContactDAL(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<LeadContact>> CreateContacts(List<LeadContact> contatos, int idLead)
        {
            List<LeadContact> contacts = new List<LeadContact>();
            foreach (LeadContact ct in contatos)
            {
                contacts.Add(await this.InsertContact(ct, idLead));
            }

            return contatos;
        }

        private async Task<LeadContact> InsertContact(LeadContact contact, int idLead)
        {
            var contatoDb = new ContactDbRepresent
            {
                Nome = contact.Name,
                Cargo = contact.JobTitle,
                Email = contact.Email,
                LeadFk = idLead
            };

            var response = await _supabase
                .From<ContactDbRepresent>()
                .Insert(contatoDb);

            var created = response.Models.FirstOrDefault();

            if (created == null)
                throw new RepositoriesException("Erro ao inserir contato.");


            int idContato = created.ContatoId;

            LeadContact leadContact = new LeadContact(created.Nome, created.Cargo, created.Email);
            leadContact.LeadNumbers.AddRange(await this.InsertNumber(contact.LeadNumbers, idContato));
            return leadContact;
        }


        private async Task<List<LeadNumber>> InsertNumber(List<LeadNumber> numeros, int idContato)
        {
            List<NumberDbRepresent> listaNumeros = new List<NumberDbRepresent>();

            if (numeros != null && numeros.Count != 0)
            {

                foreach (LeadNumber nr in numeros)
                {
                    var numeroDb = new NumberDbRepresent
                    {
                        Numero = nr.Number,
                        Tipo = nr.Type,
                        Whatsapp = nr.Whatsapp,
                        ContatoFk = idContato
                    };
                    listaNumeros.Add(numeroDb);
                }


                var response = await _supabase
                    .From<NumberDbRepresent>()
                    .Insert(listaNumeros);

                var created = response.Models.FirstOrDefault();

                if (created == null)
                    throw new RepositoriesException("Erro ao criar numero.");


                List<LeadNumber> leadNumbers = new List<LeadNumber>();
                foreach (var n in response.Models)
                {
                    leadNumbers.Add(new LeadNumber(n.Numero, n.Tipo, n.Whatsapp));
                }
                return leadNumbers;
            }
            return new List<LeadNumber>();
        }






        // ======================================= TESTANDO =======================================


        public async Task<LeadContact> UpdateLeadContactAsync(int contactId, LeadContact leadContact)
        {
            var contactDb = new ContactDbRepresent
            {
                Nome = leadContact.Name,
                Cargo = leadContact.JobTitle,
                Email = leadContact.Email,
            };

            var responseContact = await _supabase
                .From<ContactDbRepresent>()
                .Where(c => c.ContatoId == contactId)
                .Set(c => c.Nome, contactDb.Nome)
                .Set(c => c.Cargo, contactDb.Cargo)
                .Set(c => c.Email, contactDb.Email)
                .Update();

            var updatedContact = responseContact.Models.FirstOrDefault();

            if (updatedContact == null)
                throw new RepositoriesException("Erro ao atualizar os dados do contato.");

            LeadContact contact = new LeadContact(updatedContact.Nome, updatedContact.Cargo, updatedContact.Email);

            return leadContact;
        }

        public async Task<LeadNumber> UpdateLeadContactNumberAsync(int numberId, LeadNumber leadnumber)
        {


            var numberDb = new NumberDbRepresent
            {
                Numero = leadnumber.Number,
                Tipo = leadnumber.Type,
                Whatsapp = leadnumber.Whatsapp
            };


            var numberResponse = await _supabase
                .From<NumberDbRepresent>()
                .Where(n => n.NumeroId == numberId)
                .Set(n => n.Numero, numberDb.Numero)
                .Set(n => n.Tipo, numberDb.Tipo)
                .Set(n => n.Whatsapp, numberDb.Whatsapp)
                .Update();

            var updatedNumber = numberResponse.Models.FirstOrDefault();

            if (updatedNumber == null)
                throw new RepositoriesException("Erro ao atualizar os dados do contato.");


            LeadNumber number = new LeadNumber(updatedNumber.Numero, updatedNumber.Tipo, updatedNumber.Whatsapp);

            return number;
        }

        public async Task<SimpleContactUpdateResponse> SelectOneSpecificContactAsync(int contactId)
        {
            var response = await _supabase
                .From<ContactDbRepresent>()
                .Where(c => c.ContatoId == contactId)
                .Get();

            var contatoDb = response.Models.FirstOrDefault();

            if (contatoDb == null)
                throw new RepositoriesException("Contato não encontrado na base.");

            SimpleContactUpdateResponse simpleContactUpdatedResponse = new SimpleContactUpdateResponse(contatoDb.Nome, contatoDb.Cargo, contatoDb.Email);

            return simpleContactUpdatedResponse;
        }

        public async Task<SimpleNumberResponse> SelectOneSpecificContactNumberAsync(int numberId)
        {

            var responseNumbers = await _supabase
                .From<NumberDbRepresent>()
                .Where(n => n.NumeroId == numberId)
                .Get();

            var numberDb = responseNumbers.Models.FirstOrDefault();

            if (numberDb == null)
                throw new RepositoriesException("Erro ao buscar os numeros do contato.");

            SimpleNumberResponse simpleContacNumbertUpdatedResponse = new SimpleNumberResponse(numberDb.Numero, numberDb.Tipo, numberDb.Whatsapp);

            return simpleContacNumbertUpdatedResponse;
        }

        public async Task<LeadNumber> InsertContactNumberAsync(int contactId, LeadNumber leadNumber)
        {
            ArgumentNullException.ThrowIfNull(leadNumber);

            try
            {
                var response = await _supabase
                    .From<NumberDbRepresent>()
                    .Insert(new NumberDbRepresent
                    {
                        Numero = leadNumber.Number,
                        Tipo = leadNumber.Type,
                        Whatsapp = leadNumber.Whatsapp,
                        ContatoFk = contactId
                    });

                var created = response.Models.FirstOrDefault()
                    ?? throw new RepositoriesException("Não foi possível criar o número do contato.");

                return new LeadNumber(created.NumeroId, created.Numero, created.Tipo, created.Whatsapp);
            }
            catch (Exception ex) when (ex is not RepositoriesException)
            {
                throw new RepositoriesException("Erro ao criar número do contato.", ex);
            }
        }

    }
}
