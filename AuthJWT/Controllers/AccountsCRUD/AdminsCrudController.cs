using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Models.AuthModels;
using AuthJWT.Options;
using AuthJWT.Services;
using DTOs.DTOs;
using DTOs.DTOs.AuthModerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Services.AdministartionAccountsService;
using Services.Services.ModeratersAccountsService;

namespace AuthJWT.Controllers.ModeratersCRUD
{
    [Route("api/[controller]/[action]")]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [Authorize(Roles = Role.SuperAdmin)]
    [ApiController]
    public class AdminsCrudController : ControllerBase
    {
        private readonly ILogger<AdminsCrudController> logger;
        private readonly AdminsCrudService adminsCrudService;
        private readonly ModeratorsCrudService moderatorsCrudService;
        public AdminsCrudController(AdminsCrudService adminsCrudService, ModeratorsCrudService moderatorsCrudService, ILogger<AdminsCrudController> logger)
        {
            this.logger = logger;
            this.adminsCrudService = adminsCrudService;
            this.moderatorsCrudService = moderatorsCrudService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            try
            {
                List<Admin> admins = await adminsCrudService.GetAdmins();
                return Ok(new { admins });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminById(Guid id)
        {
            try
            {
                Admin admin = await adminsCrudService.GetAdminById(id);
                if (admin == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }
                return Ok(new { admin });
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin([FromBody]AdminDTO addAdminDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                ResponseDTO check = await adminsCrudService.CheckAdminExistForAdd(addAdminDTO.Login);

                if (check.Status == false)
                {
                    return StatusCode(400, check);
                }

                check = await moderatorsCrudService.CheckModeratorExistForAdd(addAdminDTO.Login);

                if (check.Status == false)
                {
                    return StatusCode(400, check);
                }

                ResponseDTO answer = await adminsCrudService.AddAdmin(addAdminDTO);
                logger.LogInformation($"Админ: {addAdminDTO.Login}, был добавлен");
                return Ok(new { answer });
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditAdmin(Guid Id, [FromBody] EditAdministrationDTO adminDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Admin existingAdmin = await adminsCrudService.CheckAdminExist(Id);

                if (existingAdmin == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }

                ResponseDTO check = await adminsCrudService.EditAdmin(existingAdmin, adminDTO);
                logger.LogInformation($"Данные админа: {existingAdmin.Login} были изменены");
                return Ok(new { check });
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(Guid Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Admin existingAdmin = await adminsCrudService.CheckAdminExist(Id);

                if (existingAdmin == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }

                ResponseDTO check = await adminsCrudService.DeleteAdmin(existingAdmin);
                logger.LogInformation($"Данные админа: {existingAdmin.Login}, были изменены");
                return Ok(new { check });
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка, пожалуйста повторите попытку позже",
                    Status = false
                });
            }
        }

    }
}