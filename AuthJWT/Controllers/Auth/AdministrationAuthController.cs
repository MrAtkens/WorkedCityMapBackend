using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Models.AuthModels;
using AuthJWT.Options;
using AuthJWT.Services;
using DTOs.DTOs;
using DTOs.DTOs.AuthModerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Services.AdministartionAccountsService;
using Services.Services.ModeratersAccountsService;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthJWT.Controllers.Auth
{
    [Route("api/[controller]/[action]")]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [ApiController]
    public class AdministrationAuthController : Controller
    {
        private readonly ILogger<AdministrationAuthController> logger;
        private readonly AdministrationAuthService administrationAuthService;
        private readonly AdminsCrudService adminCrudService;
        private readonly ModeratorsCrudService moderatorsCrudService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AdministrationAuthController(AdministrationAuthService administrationAuthService, ModeratorsCrudService moderatorsCrudService, 
            AdminsCrudService adminCrudService, ILogger<AdministrationAuthController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.administrationAuthService = administrationAuthService;
            this.adminCrudService = adminCrudService;
            this.moderatorsCrudService = moderatorsCrudService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdministartionAuthenticate([FromBody]AdministartionAuthDTO adminAuthDTO)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Admin existingAdmin = await adminCrudService.CheckAdminExist(adminAuthDTO.Login);
                Moderator existingModerator = await moderatorsCrudService.CheckModeratorExist(adminAuthDTO.Login);

                if (existingModerator == null && existingAdmin == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }
                else if (existingModerator !=null && existingAdmin == null)
                {
                    Moderator moderator = administrationAuthService.ModeratorsAuthenticate(existingModerator, adminAuthDTO.Password);
                    return Ok(new { moderator });
                }
                else if (existingModerator == null && existingAdmin != null)
                {
                    Admin admin = administrationAuthService.AdminAuthenticate(existingAdmin, adminAuthDTO.Password);
                    return Ok(new { admin });
                }
                else
                {
                    return StatusCode(500, new ResponseDTO()
                    {
                        Message = "Сообщите о проблеме администратору, данных пользователей было найдено двое.",
                        Status = false
                    });
                }
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
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

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator, SuperAdmin")]
        public async Task<IActionResult> GetAdministartionData()
        {
            try { 
                string userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                string role = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

                if (role == Role.Admin)
                {
                    Admin existingAdmin = await adminCrudService.CheckAdminExist(userId);
                    return Ok(new { existingAdmin });
                }
                else if (role == Role.Moderator)
                {
                    Moderator existingModerator = await moderatorsCrudService.CheckModeratorExist(userId);
                    return Ok(new { existingModerator  });
                }
                else
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }
            }
             catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
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
