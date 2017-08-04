using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AppUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
      

            return new OkResult();
        }
    }
}