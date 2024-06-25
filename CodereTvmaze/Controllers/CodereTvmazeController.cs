using Microsoft.AspNetCore.Mvc;
using CodereTvmaze.BLL;

namespace CodereTvmaze.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodereTvmazeController : ControllerBase
    {

        private readonly ILogger<CodereTvmazeController> _logger;

        public CodereTvmazeController(ILogger<CodereTvmazeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetShow/{id}")]
        public MainInfo GetShow(int id)
        {
            return MainInfo.GetMainInfo(id);
        }

        [HttpGet]
        [Route("GetAllShows")]
        public IEnumerable<MainInfo> GetAllShows()
        {
            return MainInfo.GetMainInfoAll().ToArray();
        }
    }
}

