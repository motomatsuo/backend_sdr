using BackEnd.Modelos.SDR.DTO;
using BackEnd.Repositorios.SDR;
using Microsoft.AspNetCore.Mvc;
using Supabase;

namespace BackEnd.Controllers.SDR
{
    [ApiController]
    [Route("[controller]")]
    public class NumerosController : ControllerBase
    {
        private readonly Client _supabase;

        public NumerosController(Client supabase)
        {
            _supabase = supabase;
        }


        /* Vou arrumar por partes
        // =========================
        // GET
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetNumero()
        {
            var response = await _supabase
                .From<NumeroDataRepresentation>()
                .Get();

            var dtoNumero = response.Models.Select(n => new NumeroDto
            {
                NumeroId = n.NumeroId,
                Numero = n.Numero,
                Tipo = n.Tipo,
                Whatsapp = n.Whatsapp,
                LeadFk = n.LeadFk
            });

            return Ok(dtoNumero);
        }

        // =========================
        // POST
        // =========================
        [HttpPost]
        public async Task<IActionResult> PostNumero([FromBody] NumeroDto dtoNumero)
        {
            if (dtoNumero == null)
                return BadRequest("Dados inválidos.");

            var numero = new NumeroDataRepresentation
            {
                Numero = dtoNumero.Numero,
                Tipo = dtoNumero.Tipo,
                Whatsapp = dtoNumero.Whatsapp,
                LeadFk = dtoNumero.LeadFk
            };

            var response = await _supabase
                .From<NumeroDataRepresentation>()
                .Insert(numero);

            var novoNumero = response.Models.FirstOrDefault();

            if (novoNumero == null)
                return StatusCode(500, "Erro ao criar número.");

            var numeroCriado = new NumeroDto
            {
                NumeroId = novoNumero.NumeroId,
                Numero = novoNumero.Numero,
                Tipo = novoNumero.Tipo,
                Whatsapp = novoNumero.Whatsapp,
                LeadFk = novoNumero.LeadFk
            };

            return CreatedAtAction(
                nameof(GetNumero),
                new { id = numeroCriado.NumeroId },
                numeroCriado
            );
        }

        // =========================
        // PUT
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNumero(int id, [FromBody] NumeroDto dtoNumero)
        {
            if (dtoNumero == null)
                return BadRequest("Dados inválidos.");

            if (id != dtoNumero.NumeroId)
                return BadRequest("Id da rota diferente do corpo da requisição.");

            // Verifica se existe
            var existing = await _supabase
                .From<NumeroDataRepresentation>()
                .Where(n => n.NumeroId == id)
                .Get();

            if (!existing.Models.Any())
                return NotFound("Número não encontrado.");

            var numero = new NumeroDataRepresentation
            {
                NumeroId = id, // 🔑 obrigatório no update
                Numero = dtoNumero.Numero,
                Tipo = dtoNumero.Tipo,
                Whatsapp = dtoNumero.Whatsapp,
                LeadFk = dtoNumero.LeadFk
            };

            var response = await _supabase
                .From<NumeroDataRepresentation>()
                .Update(numero);

            var numeroAtualizado = response.Models.FirstOrDefault();

            if (numeroAtualizado == null)
                return StatusCode(500, "Erro ao atualizar número.");

            var dtoResult = new NumeroDto
            {
                NumeroId = numeroAtualizado.NumeroId,
                Numero = numeroAtualizado.Numero,
                Tipo = numeroAtualizado.Tipo,
                Whatsapp = numeroAtualizado.Whatsapp,
                LeadFk = numeroAtualizado.LeadFk
            };

            return Ok(dtoResult);
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNumero(int id)
        {
            // Verifica se existe
            var existing = await _supabase
                .From<NumeroDataRepresentation>()
                .Where(n => n.NumeroId == id)
                .Get();

            if (!existing.Models.Any())
                return NotFound("Número não encontrado.");

            await _supabase
                .From<NumeroDataRepresentation>()
                .Where(n => n.NumeroId == id)
                .Delete();

            return NoContent(); // 204
        }
        */
    }
}