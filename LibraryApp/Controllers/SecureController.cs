using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SecureController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetData()
        {
            return Ok(new { message = "Hello Mohamed from secured controller" });
        }
    }
}
