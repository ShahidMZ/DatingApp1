using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace API.Controllers
{
    [Route("api/[controller]")] // localhost:5001/api/[controller]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ActionResult HandleError(Exception ex)
        {
            // Log the error (not implemented)
            return StatusCode(500, "Internal server error");
        }

        protected ActionResult HandleNotFound()
        {
            return NotFound("Resource not found");
        }

        protected ActionResult HandleBadRequest(string message)
        {
            return BadRequest(message);
        }

        protected ActionResult HandleUnauthorized()
        {
            return Unauthorized("Unauthorized access");
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok($"Retrieved resource with ID: {id}");
        }
    }
}
