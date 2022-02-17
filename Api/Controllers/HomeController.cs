using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API is working...");
        }
    }
}
