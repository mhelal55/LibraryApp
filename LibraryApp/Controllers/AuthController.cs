using LibraryApp.Models;
using LibraryApp.Models.Authentication;
using LibraryApp.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> delete(string id) 
                         => ( Ok(await _authServices.delete(id)));

        //public async Task<IActionResult> delete(string id)
        //{
        //    var result=  _authServices.delete(id);
        //    return Ok(result);
        //}

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync( RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authServices.RegisterAsync(model);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            //SetRefreshTokenToCookies(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(new {token=result.Token,expireOn=result.ExpireOn});
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _authServices.GetAll();

            return Ok(result);
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authServices.GetTokenAsync(model);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            //if (!string.IsNullOrEmpty(result.RefreshToken))
            //{
            //    SetRefreshTokenToCookies(result.RefreshToken, result.RefreshTokenExpiration);
            //}

            return Ok(result);
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authServices.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }

            return Ok(model);
        }

    }
}
