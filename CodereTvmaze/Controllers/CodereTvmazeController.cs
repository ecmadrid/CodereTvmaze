using Microsoft.AspNetCore.Mvc;
using CodereTvmaze.BLL;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;


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
 * Job endpoint with web api key.
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
        [Route("Import")]
        public IResult Import()
        {
            // Check api key.

            string validApiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Apikey"];

            string authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("ApiKey"))
            {
                string userApiKey = authHeader.Split(' ')[1];
            }
            else
            {
                return Results.Unauthorized();
            }

            bool rs = MainInfo.ImportMainInfoAll();

            if (rs == null)
            {
                return Results.BadRequest();
            }

            return Results.Ok();
        }

        /*
         * 
         * Public endpoints.
         * 
         */

        [HttpGet]
        [Route("Shows/{id}")]
        public IResult Shows(int id)
        {
            var obj = MainInfo.GetById(id);

            if (obj == null)
            {
                return Results.NotFound();

            }

            return Results.Ok(obj);
        }

        [HttpPost]
        [Route("Shows")]
        public IResult Shows(long page)
        {
            var objs = MainInfo.GetByPage(page).ToArray();

            if (objs == null)
            {
                return Results.NotFound();
            }

            if (objs.Length < 1)
            {
                return Results.NotFound();
            }

            return Results.Ok(objs);
        }
    }
}

