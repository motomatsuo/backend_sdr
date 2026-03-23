using Microsoft.AspNetCore.Mvc;

namespace BackEnd.API.Controllers.SDR
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public HealthController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "ok",
                environment = _environment.EnvironmentName
            });
        }
    }
}
