using Microsoft.AspNetCore.Mvc;
namespace CaseStudy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Index()
        {
            return "server started";
        }
    }
}
