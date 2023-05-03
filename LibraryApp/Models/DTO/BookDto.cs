using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.DTO
{
    public class BookDto
    {


        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Required]
        public int GenreId { get; set; }
        [Required]
        public int NumberOfBooksInStore { get; set; }
        [Required]
        public int BookPrice { get; set; }
        [Required]
        public int AuthorId { get; set; }

    }
}
