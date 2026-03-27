using BackEnd.Modelos.SDR.DTO.Receita;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BackEnd.API.Controllers.SDR
{
    [ApiController]
    [Route("api/receita")]
    public class ReceitaAwsController : ControllerBase
    {
        private readonly ReceitaAWS _receitaAWS;

        public ReceitaAwsController(ReceitaAWS receitaAWS)
        {
            _receitaAWS = receitaAWS;
        }
        [HttpGet("{cnpj}")]
        public async Task<IActionResult> GetReceitaCnpj(string cnpj)
        {
            var dados = await _receitaAWS.ConsultarCnpjAsync(cnpj);
            return Ok(dados);
        }
    }
}
