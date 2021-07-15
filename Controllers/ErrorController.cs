using CrudClients.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CrudClients.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {

        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{code}")]
        [Produces(typeof(ResponseModel<ErrorModel>))]
        public IActionResult Execute(int code)
        {
            var message = "Thrown from middleware exception redirect.";

            string errorMessage;
            try
            {
                errorMessage = ((HttpStatusCode)code).ToString();
            }
            catch
            {
                errorMessage = "Unknown";
            }

            return StatusCode(code,ResponseModel<ErrorModel>.New(code, message,ErrorModel.New(code,errorMessage)));

        }
    }
}
