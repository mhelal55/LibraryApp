using LibraryApp.Models;
using LibraryApp.Models.Data;
using LibraryApp.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace LibraryApp.Controllers
{
  //  [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var books = await _context.Books.Include(a=>a.Author).Include(a=>a.Genre).ToListAsync();
            return Ok(books);
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook([FromBody] BookDto book)
        {
            if (book == null||!ModelState.IsValid)
                return BadRequest("Data entry is wrong");
            Book result = new Book();
            result.Name = book.Name;
            result.AuthorId = book.AuthorId;
            result.NumberOfBooksInStore = book.NumberOfBooksInStore;
            result.BookPrice = book.BookPrice;
            result.GenreId = book.GenreId;
            _context.Books.Add(result);
            _context.SaveChanges();
            return Ok(book);
        }

        [HttpPut("UpdateBook/{id}")]
        public async Task<IActionResult> UpdateBook(int id,BookDto book)
        {

            if (book == null||!ModelState.IsValid)
                return BadRequest("Data entry is wrong");
            var result = await _context.Books.FirstOrDefaultAsync(a => a.Id == id);

            if (result is null)
                return BadRequest("Data entry is wrong");

            result.NumberOfBooksInStore = book.NumberOfBooksInStore;
            result.AuthorId = book.AuthorId;
            result.Name = book.Name;
            result.GenreId = book.GenreId;
            result.BookPrice = book.BookPrice;


            _context.Books.Update(result);
            _context.SaveChanges();
            return Ok(result);
        }
        [HttpGet("GetBookById/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var result = await _context.Books.Include(a => a.Author).Include(a => a.Genre).FirstOrDefaultAsync(a => a.Id == id);
            if (result is null)
                return BadRequest("Data entry is wrong");
            return Ok(result);
        }

        [HttpGet("GetByGenre/{id}")]
        public async Task<IActionResult> GetByGenreId(int id)
        {
            var result = await _context.Books.Where(m => m.GenreId == id).Include(a => a.Genre).Include(a => a.Author).ToListAsync();

            if (result is null)
                return BadRequest("Data entry is wrong");


            return Ok(result);
        }



        [HttpDelete("DeleteBook/{Id}")]
        public async Task<IActionResult> DeleteBook(int Id)
        {

            var result = await _context.Books.FirstOrDefaultAsync(a => a.Id == Id);

            if (result is null)
                return BadRequest("Data entry is wrong");

            _context.Books.Remove(result);
            _context.SaveChanges();
            return Ok(result);
        }



    }
}
