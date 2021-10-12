using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.WebApi.Controllers
{
    //http://localhost:5000/api/Home/GetHome

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController :ControllerBase
    {
        [HttpGet]
        [Route("GetHome")]
        public string GetMessage()
        {
            return "Hello";
        }
        
        [HttpGet]
        [Route("Index")]
        public string Index()
        {
            return "Index";
        }
    }
}
