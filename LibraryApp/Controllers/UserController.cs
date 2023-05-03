using LibraryApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    var users=_context.Users.ToList();
        //    return Ok(users);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateUserAsync([FromBody] ApplicationUser user)
        //{
        //    if (user == null||user.LastName==null||user.FirstName==null)
        //       return BadRequest("Data entry is wrong");

        //    _context.Users.Add(user);
        //    _context.SaveChanges();
        //    return Ok(user);
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateUserAsync(ApplicationUser user)
        //{
           
        //    if (user == null || user.LastName == null || user.FirstName == null)
        //        return BadRequest("Data entry is wrong");
        //    var result=await _context.Users.FirstOrDefaultAsync(a=>a.Id==user.Id);

        //    if (result is null)
        //        return BadRequest("Data entry is wrong");

        //    result.FirstName=user.FirstName;
        //    result.LastName=user.LastName;

        //    _context.Users.Update(result);
        //    _context.SaveChanges();
        //    return Ok(result);
        //}
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var result = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
        //    return Ok(result);
        //}


        
        //[HttpDelete]
        //public async Task<IActionResult> DeleteUserAsync([FromBody]int Id)
        //{
           
        //    var result=await _context.Users.FirstOrDefaultAsync(a=>a.Id==Id);

        //    if (result is null)
        //        return BadRequest("Data entry is wrong");

        //    _context.Users.Remove(result);
        //    _context.SaveChanges();
        //    return Ok(result);
        //}



    }
}
