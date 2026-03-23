using BackEnd.Modelos.SDR.DTO.Address;
using BackEnd.Modelos.SDR.DTO.Contact;
using BackEnd.Modelos.SDR.DTO.DataTables;
using BackEnd.Modelos.SDR.DTO.Number;
using BackEnd.Modelos.SDR.Exceptions;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Repositorios.SDR.DAL;
using BackEnd.Repositorios.SDR.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Servicos.SDR.Services
{
    public class ContactService
    {
        private LeadContact _leadContact;
        private List<LeadContact> _leadContacts = new List<LeadContact>();
        private readonly ContactDAL _contactDAL;

        public ContactService(ContactDAL contactDAL)
        {
            _contactDAL = contactDAL;
        }

        public async Task<List<SimpleContactResponse>> CadastrarContatos(List<CreateContactsRequest> createContactsRequestList, int idLead)
        {
            ConvertContactRestToLeadContact(createContactsRequestList);
            _leadContacts = await _contactDAL.CreateContacts(_leadContacts, idLead);
            return ConvertLeadContactToSimpleContactResponse(_leadContacts);
        }

        private void ConvertContactRestToLeadContact(List<CreateContactsRequest> createContactsRequestList)
        {
            foreach (var contact in createContactsRequestList)
            {
                LeadContact leadContact = new LeadContact(contact.Nome, contact.Cargo, contact.Email);
                leadContact.LeadNumbers.AddRange(contact.Numeros.Select(n => new LeadNumber(n.Numero, n.Tipo, n.Whatsapp)));
                _leadContacts.Add(leadContact);
            }
        }

        private List<SimpleContactResponse> ConvertLeadContactToSimpleContactResponse(List<LeadContact> leadContact)
        {
            var simpleContactResponses = new List<SimpleContactResponse>();

            if (leadContact == null || !leadContact.Any())
                return simpleContactResponses;

            foreach (var contact in leadContact)
            {
                var numeros = contact.LeadNumbers?
                    .Select(n => new SimpleNumberResponse(n.Number, n.Type, n.Whatsapp))
                    .ToList();

                simpleContactResponses.Add(new SimpleContactResponse(
                    contact.Name,
                    contact.JobTitle,
                    contact.Email,
                    numeros
                ));
            }

            return simpleContactResponses;
        }

       




    
        public async Task<UpdatedNumberResponse> ModifyLeadContactNumberAsync(int numberId, UpdateNumberRequest updateNumber)
        {
            try
            {
                var numberResponse = await _contactDAL.SelectOneSpecificContactNumberAsync(numberId);

                LeadNumber leadNumber = new LeadNumber
                (
                    (numberResponse.Numero != updateNumber.Numero) ? updateNumber.Numero : numberResponse.Numero,
                    (numberResponse.Tipo != updateNumber.Tipo) ? updateNumber.Tipo : numberResponse.Tipo,
                    (numberResponse.Whatsapp != updateNumber.Whatsapp) ? updateNumber.Whatsapp : numberResponse.Whatsapp
                );

                var updatedNumber = await _contactDAL.UpdateLeadContactNumberAsync(numberId, leadNumber);

                return new UpdatedNumberResponse(updatedNumber.Number, updatedNumber.Type, updatedNumber.Whatsapp);
            }
            catch (RepositoriesException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } // Completo

        public async Task<NumberRepsonse> RegisterLeadContatcNumberAsync(int contactId, CreateNumberRequest createNumber)
        {
            try
            {
                LeadNumber leadNumber = new LeadNumber(createNumber.Numero, createNumber.Tipo, createNumber.Whatsapp);
                leadNumber = await _contactDAL.InsertContactNumberAsync(contactId, leadNumber);
                return new NumberRepsonse(leadNumber.NumberId, leadNumber.Number, leadNumber.Type, leadNumber.Whatsapp);
            }
            catch (RepositoriesException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } // Completo


        public async Task<UpdatedContactResponse> ModifyLeadContactAsync(int contactId, UpdateContactRequest updateContact)
        {
            try
            {
                /*
                _addressValidator.ValidateAddressId(addressId);
                _addressValidator.ValidateAddressStreet(updateAddress.Rua);
                int number = _addressValidator.ValidateAddressNumber(updateAddress.Numero);
                _addressValidator.ValidateAddressNeighborhood(updateAddress.Bairro);
                _addressValidator.ValidateAddressCity(updateAddress.Cidade);
                _addressValidator.ValidateAddressUF(updateAddress.Uf);
                */
                var contactResponse = await _contactDAL.SelectOneSpecificContactAsync(contactId);

                LeadContact leadContact = new LeadContact
                (
                    (contactResponse.Nome != updateContact.Nome) ? updateContact.Nome : contactResponse.Nome,
                    (contactResponse.Cargo != updateContact.Cargo) ? updateContact.Cargo : contactResponse.Cargo,
                    (contactResponse.Email != updateContact.Email) ? updateContact.Email : contactResponse.Email
                );

                var contactUpdated = await _contactDAL.UpdateLeadContactAsync(contactId, leadContact);

                return new UpdatedContactResponse
                (
                    contactUpdated.Name,
                    contactUpdated.JobTitle,
                    contactUpdated.Email
                );
            }
            catch (ModelException ex)
            {
                throw new ModelException(ex.Message);
            }
            catch (RepositoriesException ex)
            {
                throw new RepositoriesException(ex.Message);
            }
        }

    }
}
