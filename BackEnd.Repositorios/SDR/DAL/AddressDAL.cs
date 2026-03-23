using BackEnd.Modelos.SDR.DTO.Address;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Repositorios.SDR.Exceptions;
using Supabase;


namespace BackEnd.Repositorios.SDR.DAL
{
    public class AddressDAL
    {
        private Client _supabase;

        public AddressDAL(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<SimpleAddressResponse>> SelectAllAddress() // Vai trazer todos os endereços existentes: FUNCIONANDO
        {
            var addressDbResponse = await _supabase
                .From<AddressDbRepresent>()
                .Get();

            var addressList = new List<SimpleAddressResponse>();
            foreach (var a in addressDbResponse.Models)
            {
                addressList.Add(new SimpleAddressResponse(a.Rua, a.Numero, a.Bairro, a.Cidade, a.UF));
            }
            return addressList;
        }

        public async Task<List<AddressResponse>> SelectSpecificAddress(int idLead) // Vai trazer todos os endereços existentes: FUNCIONANDO
        {
            var addressDbResponse = await _supabase
                .From<AddressDbRepresent>()
                .Where(e => e.LeadFk == idLead)
                .Get();

            var addressList = new List<AddressResponse>();
            foreach (var a in addressDbResponse.Models)
            {
                addressList.Add(new AddressResponse(a.EnderecoId, a.Rua, a.Numero, a.Bairro, a.Cidade, a.UF));
            }
            return addressList;
        }

        public async Task<SimpleAddressResponse> SelectOneSpecificAddress(int addressId) // Vai trazer todos os endereços existentes: FUNCIONANDO
        {
            var addressDbResponse = await _supabase
                .From<AddressDbRepresent>()
                .Where(e => e.EnderecoId == addressId)
                .Get();

            var addresses = addressDbResponse.Models.FirstOrDefault();
            if (addresses == null)
                throw new RepositoriesException("Endereço não encontrado para o Lead informado.");

            return new SimpleAddressResponse(addresses.Rua, addresses.Numero, addresses.Bairro, addresses.Cidade, addresses.UF);
        }



        public async Task<LeadAddress> UpdateLeadAddress(int addressId, LeadAddress leadAddress)
        {
            AddressDbRepresent addressDb = new AddressDbRepresent
            {
                Rua = leadAddress.Street,
                Numero = leadAddress.Number,
                Bairro = leadAddress.Neighborhood,
                Cidade = leadAddress.City,
                UF = leadAddress.UF
            };

            var response = await _supabase
                .From<AddressDbRepresent>()
                .Where(a => a.EnderecoId == addressId)
                .Set(a => a.Rua, leadAddress.Street)
                .Set(a => a.Numero, leadAddress.Number)
                .Set(a => a.Bairro, leadAddress.Neighborhood)
                .Set(a => a.Cidade, leadAddress.City)
                .Set(a => a.UF, leadAddress.UF)
                .Update();

            var AddressUpdated = response.Models.FirstOrDefault();

            if (AddressUpdated == null)
                throw new RepositoriesException("Erro ao atualizar o endereço.");

            return new LeadAddress(AddressUpdated.Rua, AddressUpdated.Numero, AddressUpdated.Bairro, AddressUpdated.Cidade, AddressUpdated.UF);
        } // Completo

        public async Task<List<LeadAddress>> InsertLeadAddress(int idLead, List<LeadAddress> addresses)
        {
            List<AddressDbRepresent> addressList = addresses
                .Select(el => new AddressDbRepresent { Rua = el.Street, Numero = el.Number, Bairro = el.Neighborhood, Cidade = el.City, UF = el.UF, LeadFk = idLead })
                .ToList();

            var response = await _supabase
            .From<AddressDbRepresent>()
            .Insert(addressList);

            var created = response.Models.FirstOrDefault();

            if (created == null)
                throw new RepositoriesException("Erro ao inserir endereço.");

            return addresses;
        } // Completo
    }
}
