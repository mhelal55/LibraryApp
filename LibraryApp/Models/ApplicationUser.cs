using LibraryApp.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

       public List<RefreshToken>? RefreshTokens { get;set; }
    //public void Update(string firstName, string lastName)
    //{
    //    this.FirstName = firstName;
    //    this.LastName = lastName;
    //}

}
}
