using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Helpers;
using AuthJWT.Models;
using AuthJWT.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthJWT.Services
{
    public class UserAuthService
    {
        private readonly UserContext context;
        private readonly string jwtSecret;
        
        public UserAuthService(UserContext context, IOptions<SecretOptions> secretOptions)
        {
            this.context = context;
            jwtSecret = secretOptions.Value.JWTSecret;
        }

        public async Task<bool> Registration(AuthDTO authDTO)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(authDTO.Password);
            User user = new User() { Phone = authDTO.Phone, Password = password, Role = Role.User };
            try
            {
                User foundedUser = await context.Users.FirstOrDefaultAsync(user => user.Phone == user.Phone);
                if(foundedUser.Phone == user.Phone)
                {
                    return false;
                }
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<User> Authenticate(AuthDTO authDTO)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(authDTO.Password);
            var existingUser = await context.Users.FirstOrDefaultAsync(user => user.Phone == authDTO.Phone && user.Password == password);
            if(existingUser == null)
            {
                existingUser.Token = null;
                return existingUser;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, existingUser.Phone),
                    new Claim(ClaimTypes.Role, existingUser.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            existingUser.Token = tokenHandler.WriteToken(token);
            return existingUser.UserWithoutPassword();
        }
    }
}
