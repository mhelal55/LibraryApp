using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.Data
{
    public class Book
    {

        public int Id { get; set; }
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
        public Genre Genre { get; set; }
        public Author Author { get; set; }


    }
}
