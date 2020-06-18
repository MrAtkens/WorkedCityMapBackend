using AuthJWT.DataAcces;
using AuthJWT.DTOs;
using AuthJWT.Helpers;
using AuthJWT.Models;
using AuthJWT.Options;
using DTOs.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Trunking.V1;

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

        public async Task<ResponseDTO> CheckUser(string phoneNumber)
        {
            User foundedUser = await context.Users.FirstOrDefaultAsync(user => user.Phone == phoneNumber);
            if(foundedUser == null)
            {
                return new ResponseDTO() { Status = true };
            }
            else
            {
                return new ResponseDTO() { Message = "Данный пользователь уже существует", Status = false };
            }
        }

        public async Task<ResponseDTO> Registration(RegistartionUserDTO registartionDTO)
        {
            var existingCode = await context.Codes.FirstOrDefaultAsync(code => code.PhoneNumber == registartionDTO.Phone && code.Code == registartionDTO.VerificationCode);
            if(existingCode == null)
            {
                return new ResponseDTO() { Message = "Данный верификационный код не найден либо не верен", Status = false };
            }
            else
            {
                context.Codes.Remove(existingCode);
                await context.SaveChangesAsync();
            }
            string password = BCrypt.Net.BCrypt.HashPassword(registartionDTO.Password);
            User user = new User() { Phone = registartionDTO.Phone, Password = password, Role = Role.User };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new ResponseDTO() { Message = "Вы успешно зарегистрировались", Status = true };
        }
        public async Task<User> Authenticate(AuthUserDTO authDTO)
        {
            User existingUser = await context.Users.FirstOrDefaultAsync(user => user.Phone == authDTO.Phone);
            if (existingUser == null)
            {
                return existingUser;
            }
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(authDTO.Password, existingUser.Password);
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

        public async Task<ResponseDTO> PasswordChange(RegistartionUserDTO registartionDTO)
        {
            var existingCode = await context.Codes.FirstOrDefaultAsync(code => code.PhoneNumber == registartionDTO.Phone && code.Code == registartionDTO.VerificationCode);
            if (existingCode == null)
            {
                return new ResponseDTO() { Message = "Данный верификационный код не найден либо не верен", Status = false };
            }
            else
            {
                context.Codes.Remove(existingCode);
                await context.SaveChangesAsync();
            }
            User foundedUser = await context.Users.FirstOrDefaultAsync(user => user.Phone == registartionDTO.Phone);
            
            if(foundedUser == null)
            {
                return new ResponseDTO() { Message = "Данный пользователь не найден", Status = false };
            }
            User newPasswordUser = foundedUser;
            newPasswordUser.Password = registartionDTO.Password;
            context.Entry(foundedUser).CurrentValues.SetValues(newPasswordUser);
            await context.SaveChangesAsync();
            return new ResponseDTO() { Message = "Пароль успешно изменён", Status = true};
        }
    }
}
