using Microsoft.AspNetCore.Mvc;
using CodereTvmaze.BLL;
using System.Collections.Generic;

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

        /*
 * 
 * Job endpoints with web api key.
 * 
 */

        [HttpGet]
        [Route("Import/{id}")]
        public IResult ImportShow(int id)
        {
            //return MainInfo.GetMainInfo(id);

            MainInfo mainInfo = MainInfo.ImportMainInfo(id);

            if (mainInfo == null)
            {
                return Results.BadRequest(null);
            }

            return Results.Ok(mainInfo);
        }

        [HttpGet]
        [Route("ImportAllShows")]
        public IResult ImportAllShows()
        {

            IEnumerable<MainInfo> mainInfos = MainInfo.ImportMainInfoAll().ToArray();

            if (mainInfos == null)
            {
                return Results.BadRequest(null);
            }

            return Results.Ok(mainInfos);
        }

        /*
         * 
         * Public endpoints.
         * 
         */

        [HttpGet]
        [Route("GetShow/{id}")]
        public MainInfo GetShow(int id)
        {
            var obj = MainInfo.GetById(id);
            return obj;
        }

        [HttpGet]
        [Route("GetAllShows")]
        public IEnumerable<MainInfo> GetAllShows()
        {
            return MainInfo.GetAll().ToArray();
        }
    }
}

