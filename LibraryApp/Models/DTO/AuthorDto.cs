using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.DTO
{
    public class AuthorDto
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
