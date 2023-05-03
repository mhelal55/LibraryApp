using LibraryApp.Models;
using LibraryApp.Models.Authentication;
using LibraryApp.Models.DTO;

namespace LibraryApp.Services.Abstract
{
    public interface IAuthServices
    {
        Task<AuthModel>RegisterAsync(RegisterModel model);
        Task<AuthModel>delete(string id);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);

        Task<string> AddRoleAsync(AddRoleModel model);


        Task<List<ApplicationUser>> GetAll();


    }
}
