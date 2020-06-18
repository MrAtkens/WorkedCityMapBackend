using AuthJWT.DataAcces;
using AuthJWT.Helpers;
using AuthJWT.Models;
using AuthJWT.Models.AuthModels;
using DTOs.DTOs;
using DTOs.DTOs.AuthModerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services.AdministartionAccountsService
{
    public class UsersCrudService
    {
        private readonly IMemoryCache cache;
        private readonly UserContext context;
        public UsersCrudService(IMemoryCache cache, UserContext context)
        {
            this.cache = cache;
            this.context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            List<User> users = await context.Users.ToListAsync();
            return users.UsersWithoutPasswords().ToList();
        }

        public async Task<User> GetUserById(Guid id)
        {
            User user = null;
            if (!cache.TryGetValue(id, out user))
            {
                user = await context.Users.FirstOrDefaultAsync(userFound => userFound.Id == id);
                if (user != null)
                {
                    cache.Set(user.Id, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return user;
        }

        public async Task<ResponseDTO> EditUser(User foundedUser, EditModeratorDTO editModeratorDTO)
        {
            if (editModeratorDTO.Password != null)
            {
                string password = BCrypt.Net.BCrypt.HashPassword(editModeratorDTO.Password);
                editModeratorDTO.Password = password;
            }
            context.Entry(foundedUser).CurrentValues.SetValues(editModeratorDTO);
            int count = await context.SaveChangesAsync();
            if (count > 0)
            {
                cache.Set(foundedUser.Id, foundedUser, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return new ResponseDTO() { Message = "Информация пользователя изменена успешно", Status = true };
        }

        public async Task<ResponseDTO> DeleteUser(User user)
        {
            User bufferUser = user;
            if (cache.TryGetValue(user.Id, out bufferUser))
            {
                cache.Remove(user.Id);
            }
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return new ResponseDTO() { Message = "Пользователь успешно удалён", Status = true }; ;
        }

        //For AddModerator function
        public async Task<ResponseDTO> CheckModeratorExistForAdd(string phone)
        {
            User foundedUser = await context.Users.FirstOrDefaultAsync(moderator => moderator.Phone == phone);
            if (foundedUser == null)
            {
                return new ResponseDTO() { Status = true };
            }
            else
            {
                return new ResponseDTO() { Message = "Данный пользователь уже существует", Status = false };
            }
        }


        //For Authenticate,
        public async Task<User> CheckModeratorExist(string phone)
        {
            User foundedUser = await context.Users.FirstOrDefaultAsync(moderator => moderator.Phone == phone);
            if (foundedUser == null)
            {
                return null;
            }
            else
            {
                return foundedUser;
            }
        }

        public async Task<Moderator> CheckModeratorExist(Guid id)
        {
            Moderator foundedModerator = await context.Moderators.FirstOrDefaultAsync(moderator => moderator.Id == id);
            if (foundedModerator == null)
            {
                return null;
            }
            else
            {
                return foundedModerator;
            }
        }
    }
}
