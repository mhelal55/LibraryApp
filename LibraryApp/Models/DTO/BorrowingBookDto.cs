using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.DTO
{
    public class BorrowingBookDto
    {

        [Required]
        public DateTime DateOfBorrowing { get; set; }
        [Required]
        public DateTime EndOfBorrowing { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public int BookId { get; set; }
    }
}
