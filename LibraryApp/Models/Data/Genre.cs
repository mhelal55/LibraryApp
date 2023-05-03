using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.Data
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
