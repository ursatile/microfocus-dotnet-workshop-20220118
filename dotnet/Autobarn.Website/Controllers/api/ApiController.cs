using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase {
        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult Welcome() {
            var greeting = new {
                _links = new {
                    vehicles = new {
                        href = "/api/vehicles"
                    }
                },
                _actions = new {
                    create = new {
                        href = "/api/vehicles",
                        method = "POST",
                        name = "Create a new vehicle",
                        type = "application/json"
                    }
                },
                message = "Welcome to the Autobarn API"
            };
            return Ok(greeting);
        }


    }
}
