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
using System.Threading.Tasks;

namespace Services.Services.ModeratersAccountsService
{
    public class AdminsCrudService
    {
        private readonly IMemoryCache cache;
        private readonly ModerateContext context;

        public AdminsCrudService(ModerateContext context, IMemoryCache cache)
        {
            this.context = context;
            this.cache = cache;
        }

        public async Task<List<Admin>> GetAdmins()
        {
            List<Admin> admins = await context.Admins.ToListAsync();
            return admins.AdminsWithoutPasswords();
        }

        public async Task<Admin> GetAdminById(Guid id)
        {
            Admin admin = null;
            if (!cache.TryGetValue(id, out admin))
            {
                admin = await context.Admins.FirstOrDefaultAsync(admin => admin.Id == id);
                if (admin != null)
                {
                    cache.Set(admin.Id, admin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return admin.AdminWithoutPassword();
        }

        public async Task<ResponseDTO> AddAdmin(AdminDTO addAdminDTO)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(addAdminDTO.Password);
            Admin admin = new Admin()
            {
                Login = addAdminDTO.Login,
                Password = password,
                LastName = addAdminDTO.LastName,
                FirstName = addAdminDTO.FirstName,
                Role = Role.Admin
            };
            context.Admins.Add(admin);
            await context.SaveChangesAsync();
            return new ResponseDTO() { Message = $"Вы успешно добавили {admin.LastName} {admin.FirstName} в роли админа", Status = true };
        }


        public async Task<ResponseDTO> EditAdmin(Admin existingAdmin, EditAdministrationDTO editAdminDTO)
        {
            if(editAdminDTO.Password != null)
            {
                string password = BCrypt.Net.BCrypt.HashPassword(editAdminDTO.Password);
                editAdminDTO.Password = password;
            }
            context.Entry(existingAdmin).CurrentValues.SetValues(editAdminDTO);
            int count = await context.SaveChangesAsync();
            if (count > 0)
            {
                cache.Set(existingAdmin.Id, existingAdmin, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }
            return new ResponseDTO() { Message = "Информация пользователя изменена успешно", Status = true };
        }

        public async Task<ResponseDTO> DeleteAdmin(Admin admin)
        {
            Admin bufferAdmin = admin;
            if (cache.TryGetValue(admin.Id, out bufferAdmin))
            {
                cache.Remove(admin.Id);
            }
            context.Admins.Remove(admin);
            await context.SaveChangesAsync();
            return new ResponseDTO() { Message = "Пользователь успешно удалён", Status = true }; ;
        }


        //For AddAdmins function
        public async Task<ResponseDTO> CheckAdminExistForAdd(string login)
        {
            Admin foundedAdmin = await context.Admins.FirstOrDefaultAsync(admin => admin.Login == login);
            if (foundedAdmin == null)
            {
                return new ResponseDTO() { Status = true };
            }
            else
            {
                return new ResponseDTO() { Message = "Данный пользователь уже существует", Status = false };
            }
        }


        //For Authenticate,
        public async Task<Admin> CheckAdminExist(string login)
        {
            Admin foundedAdmin = await context.Admins.FirstOrDefaultAsync(admin => admin.Login == login);
            if (foundedAdmin == null)
            {
                return null;
            }
            else
            {
                return foundedAdmin;
            }
        }

        public async Task<Admin> CheckAdminExist(Guid id)
        {
            Admin foundedAdmin = await context.Admins.FirstOrDefaultAsync(admin => admin.Id == id);
            if (foundedAdmin == null)
            {
                return null;
            }
            else
            {
                return foundedAdmin;
            }
        }

    }
}
