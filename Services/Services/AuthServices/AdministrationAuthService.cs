using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Helpers;
using AuthJWT.Models.AuthModels;
using AuthJWT.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthJWT.Services
{
    public class AdministrationAuthService
    {
        private readonly string jwtSecret;

        public AdministrationAuthService(IOptions<SecretOptions> secretOptions)
        {
            jwtSecret = secretOptions.Value.JWTSecret;
        }
        public Admin AdminAuthenticate(Admin existingAdmin, string password)
        {
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, existingAdmin.Password);
            if (!isPasswordCorrect)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, existingAdmin.Login),
                    new Claim(ClaimTypes.Role, existingAdmin.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            existingAdmin.Token = tokenHandler.WriteToken(token);
            return existingAdmin.AdminWithoutPassword();
        }


        public Moderator ModeratorsAuthenticate(Moderator existingModerator, string password)
        {
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, existingModerator.Password);
            if (!isPasswordCorrect)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, existingModerator.Login),
                    new Claim(ClaimTypes.Role, existingModerator.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            existingModerator.Token = tokenHandler.WriteToken(token);
            return existingModerator.ModeratorWithoutPassword();
        }


    }
}
