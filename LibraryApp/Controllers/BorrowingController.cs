using LibraryApp.Models;
using LibraryApp.Models.Data;
using LibraryApp.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BorrowingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var books =await _context.BorrowingBooks.Include(a=>a.Book).Include(u=>u.ApplicationUser).ToListAsync();
            return Ok(books);
        }

        [HttpPost("AddBorrowing")]
        public async Task<IActionResult> AddBorrowing([FromBody] BorrowingBookDto model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Data entry is wrong");
            BorrowingBookModel result = new BorrowingBookModel();
            result.ApplicationUserId = model.ApplicationUserId;
            result.BookId = model.BookId;
            result.DateOfBorrowing = DateTime.UtcNow;
            result.EndOfBorrowing = DateTime.UtcNow.AddDays(15);
           var book=await _context.Books.FirstOrDefaultAsync(a => a.Id == result.BookId);
            book.NumberOfBooksInStore = book.NumberOfBooksInStore - 1;
            if (book.NumberOfBooksInStore < 1)
            {
                return BadRequest("books is not enough");
            }
            _context.Books.Update(book);
            _context.BorrowingBooks.Add(result);
            _context.SaveChanges();
            return Ok(model);
        }

        [HttpPut("UpdateBorrowing/{Id}")]
        public async Task<IActionResult> UpdateBorrowing(int Id ,BorrowingBookDto model)
        {

            if (model == null || !ModelState.IsValid)
                return BadRequest("Data entry is wrong");
            var result = await _context.BorrowingBooks.FirstOrDefaultAsync(a => a.Id == Id);

            if (result is null)
                return BadRequest("Data entry is wrong");



            var book = await _context.Books.FirstOrDefaultAsync(a => a.Id == model.BookId);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == model.ApplicationUserId);
            result.ApplicationUserId = model.ApplicationUserId;
            result.BookId = model.BookId;
            result.ApplicationUser = user;
            result.Book = book;
            result.DateOfBorrowing = model.DateOfBorrowing;
            result.EndOfBorrowing = model.EndOfBorrowing;


            _context.BorrowingBooks.Update(result);
            _context.SaveChanges();
            return Ok(result);
        }
        [HttpGet("GetBorrowingById/{id}")]
        public async Task<IActionResult> GetBorrowingById(int id)
        {
            var result = await _context.BorrowingBooks.Where(a => a.Id == id).Include(a => a.ApplicationUser).Include(a => a.Book).ToListAsync();
            if (result is null)
                return BadRequest("Data entry is wrong");
            return Ok(result);
        }

        [HttpGet("GetByUserId/{id}")]
        public async Task<IActionResult> GetByUserId(string id)
        {
            var result = await _context.BorrowingBooks.Where(m => m.ApplicationUserId == id).Include(a => a.ApplicationUser).Include(a => a.Book).ToListAsync();

            if (result is null)
                return BadRequest("Data entry is wrong");


            return Ok(result);
        }  


        [HttpGet("GetByBookId/{id}")]
        public async Task<IActionResult> GetByBookId(int id)
        {
            var result = await _context.BorrowingBooks.Where(m => m.BookId == id).Include(a => a.ApplicationUser).Include(a => a.Book).ToListAsync();

            if (result is null)
                return BadRequest("Data entry is wrong");


            return Ok(result);
        }



        [HttpDelete("DeleteBorrowing/{Id}")]
        public async Task<IActionResult> DeleteBorrowing(int Id)
        {

            var result = await _context.BorrowingBooks.FirstOrDefaultAsync(a => a.Id == Id);

            if (result is null)
                return BadRequest("Data entry is wrong");
            var book = await _context.Books.FirstOrDefaultAsync(a => a.Id == result.BookId);
            book.NumberOfBooksInStore = book.NumberOfBooksInStore + 1;
            _context.Update(book);
            _context.BorrowingBooks.Remove(result);
            _context.SaveChanges();
            return Ok(result);
        }
    }
}
