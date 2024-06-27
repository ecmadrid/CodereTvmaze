using Microsoft.AspNetCore.Mvc;
using CodereTvmaze.BLL;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;


namespace CodereTvmaze.Controllers
{
    /// <summary>
    /// ApiControler: <c>ControllerBase</c> contains web api calls. 
    /// </summary>
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
        /// <summary>
        /// Endpoint: <c>Import/id</c> imports from Tvmaze a show into database by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint: <c>Import</c> imports show from Tvmazeinto database from last inserted id.
        /// It needs that "Authotization" header contains api key ("Authotization": "ApiKey xxxxxx").
        /// Correct api key is stored in json project config file.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Import")]
        public IResult Import()
        {
            // Check api key.

            string? validApiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Apikey"];

            string? authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("ApiKey"))
            {
                string userApiKey = authHeader.Split(' ')[1];
            }
            else
            {
                return Results.Unauthorized();
            }

            bool rs = MainInfo.ImportMainInfoAll();

            if (rs == false)
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

        /// <summary>
        /// EndPoint: <c>Shows/id</c> returns a MainInfo object based on its id as parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// EndPoint: <c>Shows</c> returns MainInfo object array based on a page as parameter.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
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

