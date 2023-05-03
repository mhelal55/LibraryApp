using LibraryApp.Helpers;
using LibraryApp.Models;
using LibraryApp.Models.Authentication;
using LibraryApp.Models.DTO;
using LibraryApp.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryApp.Services.Implementation
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        private readonly JWT _jwt;

        public AuthServices(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null || !await _roleManager.RoleExistsAsync("Admin"))
            {
                return "Invalid userId or role";
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return "user is already assign to this role";

            }
            var result = await _userManager.AddToRoleAsync(user, "Admin");

            return result.Succeeded ? string.Empty : "something went wrong";
        }

        public async Task<AuthModel> delete(string id)
        {
            var user=await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return new AuthModel { Message = "Email is not exsit" };
            }
           // _userManager.remo(user);
             _context.Remove(user);
           await _context.SaveChangesAsync();

            return new AuthModel { Message = "deleted" };


        }

        public async Task<List<ApplicationUser>> GetAll()
        {
            var data = await _context.Users.ToListAsync();

            return data;


        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);



            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect";
                return authModel;

            }

            var JwtSecurityToken = await CreateJwtToken(user);

            var roleResult = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpireOn = JwtSecurityToken.ValidTo;
            authModel.Roles = roleResult.ToList();


            //if (user.RefreshTokens.Any(t => t.IsActived))
            //{
            //    var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActived);
            //    authModel.RefreshToken = activeRefreshToken.Token;
            //    authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            //}
            //else
            //{
            //    var refreshToken = GenerateRefreshToken();
            //    authModel.RefreshToken = refreshToken.Token;
            //    authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
            //    user.RefreshTokens.Add(refreshToken);
            //    await _userManager.UpdateAsync(user);

            //}


            return authModel;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { Message = "Email is already exsit" };
            }

            if (await _userManager.FindByNameAsync(model.USerName) is not null)
            {
                return new AuthModel { Message = "USerName is already exsit" };
            }

            var user = new ApplicationUser
            {
                UserName = model.USerName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Passsword);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description} , ";
                }
                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var JwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                Email = user.Email,
                ExpireOn = JwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
                UserName = user.UserName,
            };
        }


        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
