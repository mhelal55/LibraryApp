using LibraryApp.Models;
using LibraryApp.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenreController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _context.Genres.ToListAsync();
            return Ok(genres);
        }

        [HttpPost("AddGenre")]
        public async Task<IActionResult> CreateGenreAsync(Author genre)
        {
            if (genre == null)
                return BadRequest("Data entry is wrong");

            _context.Genres.Add(new Genre { Name=genre.Name});
            _context.SaveChanges();
            var result = await _context.Genres.FirstOrDefaultAsync(a => a.Name == genre.Name);

            return Ok(result);
        }

        [HttpPut("UpdateGenre")]
        public async Task<IActionResult> UpdateGenreAsync(Genre genre)
        {

            if (genre == null)
                return BadRequest("Data entry is wrong");
            var result = await _context.Genres.FirstOrDefaultAsync(a => a.Id == genre.Id);

            if (result is null)
                return BadRequest("Data entry is wrong");

            result.Name = genre.Name;

            _context.Genres.Update(result);
            _context.SaveChanges();
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Genres.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(result);
        }



        [HttpDelete("DeleteGenre/{Id}")]
        public async Task<IActionResult> DeleteGenreAsync( int Id)
        {

            var result = await _context.Genres.FirstOrDefaultAsync(a => a.Id == Id);

            if (result is null)
                return BadRequest("Data entry is wrong");

            _context.Genres.Remove(result);
            _context.SaveChanges();
            return Ok(result);
        }



    }
}
