using BackEnd.Modelos.SDR.DTO;
using BackEnd.Modelos.SDR.DTO.Address;
using BackEnd.Modelos.SDR.Exceptions;
using BackEnd.Modelos.SDR.Modelos;
using BackEnd.Repositorios.SDR;
using BackEnd.Repositorios.SDR.Exceptions;
using BackEnd.Servicos.SDR.Services;
using Microsoft.AspNetCore.Mvc;
using Supabase;

namespace BackEnd.Controllers.SDR
{
    [ApiController]
    [Route("api/endereco")]
    public class AddessesController : ControllerBase
    {
        /// <summary>
        /// Padrão de nomeclatura dos métodos e mapeamento:
        /// 
        /// Camada     -> | Controller | Serviço   | Repositório |
        ///               |            |           |             |
        /// --------------|------------|-----------|-------------|
        /// Mapeamento -> | GET        | Retrieve  | Select      |
        /// Mapeamento -> | POST       | Register  | Insert      |
        /// Mapeamento -> | PUT        | Modify    | Update      |
        /// Mapeamento -> | DELETE     | Remove    | Delete      |
        /// 
        /// </summary>

        private readonly Client _supabase;
        private readonly AddressService _addressService;

        public AddessesController(Client supabase, AddressService addressService)
        {
            _supabase = supabase;
            _addressService = addressService;
        }

        [HttpGet] // FUNCIONANDO, falta tratar erros e colocar na classe de serviço.
        public async Task<IActionResult> GetAddresses()
        {
            try
            {
                var addresses = await _addressService.GetAllAddresses();
                return Ok(addresses);
            }
            catch (Exception)
            {
                return BadRequest("Erro ao obter endereços.");
            }
        }

        [HttpGet("{idLead}")]
        public async Task<IActionResult> GetSpecificAddress(int idLead)
        {
            try
            {
                var addresses = await _addressService.GetSpecificAddress(idLead);
                return Ok(addresses);
            }
            catch (Exception)
            {
                return BadRequest("Erro ao obter endereços.");
            }
        }


        [HttpPut("{addressId}")]
        public async Task<IActionResult> PutAddress(int addressId, [FromBody] UpdateAddressRequest updateAddressRequest)
        {
            try
            {
                var addressUpdated = await _addressService.ModifyLeadAddress(addressId, updateAddressRequest);
                return Ok(addressUpdated);
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro ao inserir os dados: {ex.Message}");
            }
        }


        [HttpPost("{leadId}")]
        public async Task<IActionResult> PostAddresses(int leadId, [FromBody] List<CreateAddressRequest> createAddressList)
        {
            try
            {
                var addresses = await _addressService.RegisterLeadAddresses(leadId, createAddressList);
                return Ok(addresses);
            }
            catch (ModelException ex)
            {
                return BadRequest($"Erro no envio dos dados: {ex.Message}");
            }
            catch (RepositoriesException ex)
            {
                return NotFound($"Erro ao inserir os dados: {ex.Message}");
            }
        }


        /*
     


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEndereco(int id)
        {
            // Verifica se existe
            var existing = await _supabase
                .From<EnderecoDataRepresentation>()
                .Where(e => e.EnderecoId == id)
                .Get();

            if (!existing.Models.Any())
                return NotFound("Endereço não encontrado.");

            await _supabase
                .From<EnderecoDataRepresentation>()
                .Where(e => e.EnderecoId == id)
                .Delete();

            return NoContent(); // 204
        }
        */
    }
}
