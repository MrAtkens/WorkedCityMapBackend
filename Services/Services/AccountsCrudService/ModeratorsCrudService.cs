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
    public class ModeratorsCrudService
    {
        private readonly IMemoryCache cache;
        private readonly ModerateContext context;
        public ModeratorsCrudService(IMemoryCache cache, ModerateContext context)
        {
            this.cache = cache;
            this.context = context;
        }

        public async Task<List<Moderator>> GetModerators()
        {
            List<Moderator> moderators = await context.Moderators.ToListAsync();
            return moderators.ModeratorsWithoutPasswords().ToList();
        }

        public async Task<Moderator> GetModeratorById(Guid id)
        {
            Moderator moderator = null;
            if (!cache.TryGetValue(id, out moderator))
            {
                moderator = await context.Moderators.FirstOrDefaultAsync(moderatorFound => moderatorFound.Id == id);
                if (moderator != null)
                {
                    cache.Set(moderator.Id, moderator, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return moderator;
        }

        public async Task<ResponseDTO> AddModerator(Admin admin, ModeratorDTO addModeratorDTO)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(addModeratorDTO.Password);
            Moderator moderator = new Moderator()
            {
                Login = addModeratorDTO.Login,
                Password = password,
                LastName = addModeratorDTO.LastName,
                FirstName = addModeratorDTO.FirstName,
                Role = Role.Moderator
            };
            context.Moderators.Add(moderator);
            int savedCount = await context.SaveChangesAsync();
            if(savedCount > 0)
            {
                Admin newAdmin = admin;
                newAdmin.AddedModerators += 1;
                context.Entry(admin).CurrentValues.SetValues(newAdmin);
                await context.SaveChangesAsync();
            }
            return new ResponseDTO() { Message = $"Вы успешно добавили {moderator.LastName} {moderator.FirstName} в роли модератора", Status = true, ResponseData = moderator };
        }

        public async Task<ResponseDTO> EditModerator(Moderator foundedModerator, EditModeratorDTO editModeratorDTO)
        {
            if (editModeratorDTO.Password != null)
            {
                string password = BCrypt.Net.BCrypt.HashPassword(editModeratorDTO.Password);
                editModeratorDTO.Password = password;
            }
            else
            {
                editModeratorDTO.Password = foundedModerator.Password;
            }
            context.Entry(foundedModerator).CurrentValues.SetValues(editModeratorDTO);
            int count = await context.SaveChangesAsync();
            if (count > 0)
            {
                cache.Set(foundedModerator.Id, foundedModerator, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return new ResponseDTO() { Message = "Информация пользователя изменена успешно", Status = true };
        }

        public async Task<ResponseDTO> DeleteModerator(Moderator moderator)
        {
            Moderator bufferModerator = moderator;
            if (cache.TryGetValue(moderator.Id, out bufferModerator))
            {
                cache.Remove(moderator.Id);
            }
            context.Moderators.Remove(moderator);
            await context.SaveChangesAsync();
            return new ResponseDTO() { Message = "Пользователь успешно удалён", Status = true }; ;
        }

        //For AddModerator function
        public async Task<ResponseDTO> CheckModeratorExistForAdd(string login)
        {
            Moderator foundedModerator = await context.Moderators.FirstOrDefaultAsync(moderator => moderator.Login == login);
            if (foundedModerator == null)
            {
                return new ResponseDTO() { Status = true };
            }
            else
            {
                return new ResponseDTO() { Message = "Данный пользователь уже существует", Status = false };
            }
        }


        //For Authenticate,
        public async Task<Moderator> CheckModeratorExist(string login)
        {
            Moderator foundedModerator = await context.Moderators.FirstOrDefaultAsync(moderator => moderator.Login == login);
            if (foundedModerator == null)
            {
                return null;
            }
            else
            {
                return foundedModerator;
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
