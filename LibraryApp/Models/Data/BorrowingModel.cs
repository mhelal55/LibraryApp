using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.Data
{
    public class BorrowingBookModel
    {
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime DateOfBorrowing { get; set; }
        [Required]
        public DateTime EndOfBorrowing { get; set; }
    }
}
