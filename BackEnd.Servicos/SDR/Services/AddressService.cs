using BackEnd.Modelos.SDR.DTO.Address;
using BackEnd.Modelos.SDR.Exceptions;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Repositorios.SDR.DAL;
using BackEnd.Repositorios.SDR.Exceptions;
using BackEnd.Servicos.SDR.Interfaces;


namespace BackEnd.Servicos.SDR.Services
{
    public class AddressService
    {
        private readonly AddressDAL _addressDAL;
        private readonly IAddressValidator _addressValidator;

        public AddressService(AddressDAL addressDAL, IAddressValidator addressValidator)
        {
            _addressDAL = addressDAL;
            _addressValidator = addressValidator;
        }

        public async Task<List<SimpleAddressResponse>> GetAllAddresses()
        {
            return await _addressDAL.SelectAllAddress();
        }

        public async Task<List<AddressResponse>> GetSpecificAddress(int idLead) // FALTA VALIDAR O ID LEAD ANTES DE CHAMAR O MÉTODO DO DAL
        {
            return await _addressDAL.SelectSpecificAddress(idLead);
        }

        public async Task<SimpleAddressResponse> ModifyLeadAddress(int addressId, UpdateAddressRequest updateAddress)
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
                var addressResponse = await _addressDAL.SelectOneSpecificAddress(addressId);

                LeadAddress leadAddress = new LeadAddress
                (
                    (updateAddress.Rua != addressResponse.Rua) ? updateAddress.Rua : addressResponse.Rua,
                    (updateAddress.Numero != addressResponse.Numero) ? updateAddress.Numero : addressResponse.Numero,
                    (updateAddress.Bairro != addressResponse.Bairro) ? updateAddress.Bairro : addressResponse.Bairro,
                    (updateAddress.Cidade != addressResponse.Cidade) ? updateAddress.Cidade : addressResponse.Cidade,
                    (updateAddress.Uf != addressResponse.Uf) ? updateAddress.Uf : addressResponse.Uf
                );

                var addressUpdated = await _addressDAL.UpdateLeadAddress(addressId, leadAddress);

                return new SimpleAddressResponse
                (
                    addressUpdated.Street,
                    addressUpdated.Number,
                    addressUpdated.Neighborhood,
                    addressUpdated.City,
                    addressUpdated.UF
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

        public async Task<List<SimpleAddressResponse>> RegisterLeadAddresses(int leadId, List<CreateAddressRequest> createAddressList)
        {
            List<LeadAddress> leadAddresses = createAddressList.Select(address => new LeadAddress
            (
                address.Rua,
                address.Numero,
                address.Bairro,
                address.Cidade,
                address.Uf
            )).ToList();

            var addresses = await _addressDAL.InsertLeadAddress(leadId, leadAddresses);

            List<SimpleAddressResponse> addressResponses = addresses.Select(address => new SimpleAddressResponse
            (
                address.Street,
                address.Number,
                address.Neighborhood,
                address.City,
                address.UF
            )).ToList();
            return addressResponses;
        }
    }
}
