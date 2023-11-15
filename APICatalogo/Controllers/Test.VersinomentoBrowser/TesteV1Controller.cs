using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}/teste")] // CHAMADA: https://localhost:7081/api/v1/teste
    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>TesteV1Controller - V 1.0 </h2></body></html>", "text/html");
        }
    }
}