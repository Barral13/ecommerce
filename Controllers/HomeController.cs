using ecommerce.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            string message = "Bem-vindo ao nosso e-commerce! " +
                             "Para começar, por favor alterar o HTTP: " +
                             "http://localhost:sua-porta/swagger/index.html";

            return Ok(new { message });
        }
    }
}
