using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPDSS.DataLayer.Repositories;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RPDSS.Controllers.API
{
    [Produces("application/json")]
    [Route("api/home")]
    public class HomeAPIController : Controller
    {
        private HomeRepository _model;

        public HomeAPIController(IConfiguration config)
        {
            _model = new HomeRepository(config);
        }

        // GET: api/values
        [HttpGet("graphs")]
        public IActionResult Get(int year, int growthstages)
        {
            return Ok(_model.GetGraphsData(year, growthstages));
        }

        // GET: api/values
        [HttpGet("calendar")]
        public IActionResult Get(int ricevariety, DateTime startDate)
        {
            return Ok(_model.GetCropCalendar(ricevariety, startDate));
        }
    }
}
