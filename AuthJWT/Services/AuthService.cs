using AuthJWT.DataAcces;
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
    public class AuthService
    {
        private readonly UserContext context;
        private readonly string jwtSecret;
        
        public AuthService(UserContext context, IOptions<SecretOptions> secretOptions)
        {
            this.context = context;
            jwtSecret = secretOptions.Value.JWTSecret;
        }

        public async Task<bool> Registration(string login, string password)
        {
            User user = new User() { Username = login, Password = password };
            try
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<string> Authenticate(string login, string password)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(user => user.Password == password && user.Username == login);
            if(existingUser == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
