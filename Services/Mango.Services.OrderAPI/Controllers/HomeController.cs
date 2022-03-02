using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.OrderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {

        }

        [HttpGet]
        public ActionResult<bool> Get()
        {
            return true;
        }
    }
}