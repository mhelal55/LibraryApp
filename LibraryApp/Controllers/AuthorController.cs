using LibraryApp.Models;
using LibraryApp.Models.Data;
using LibraryApp.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var Author = await _context.Authors.AsNoTracking().ToListAsync();
            return Ok(Author);
        }

        [HttpPost("addAuthor")]
        public async Task<IActionResult> CreateAuthorAsync( AuthorDto Author)
        {
            if (Author == null)
                return BadRequest("Data entry is wrong");
            _context.Authors.Add(new Author { Name = Author.Name });
            _context.SaveChanges();

            var result = await _context.Authors.FirstOrDefaultAsync(a => a.Name == Author.Name);
            return Ok(result);
        }

        [HttpPut("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthorAsync(Author Author)
        {

            if (Author == null)
                return BadRequest("Data entry is wrong");
            var result = await _context.Authors.FirstOrDefaultAsync(a => a.Id == Author.Id);

            if (result is null)
                return BadRequest("Data entry is wrong");

            result.Name = Author.Name;

            _context.Authors.Update(result);
            _context.SaveChanges();
            return Ok(result);
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (result is null)
                return BadRequest("Data entry is wrong");
            return Ok(result);
        }



        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> DeleteAuthorAsync( int Id)
        {

            var result = await _context.Authors.FirstOrDefaultAsync(a => a.Id == Id);

            if (result is null)
                return BadRequest("Data entry is wrong");

            _context.Authors.Remove(result);
            _context.SaveChanges();
            return Ok(result);
        }



    }
}
