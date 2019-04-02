using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Controllers
{
    public class SampleController : ControllerBase
    {

        public IActionResult Get()
        {
            return Ok(new {from="API"});
        }

        public IActionResult Test()
        {
            return Ok(new { from = "TEST" });
        }
    }
}
