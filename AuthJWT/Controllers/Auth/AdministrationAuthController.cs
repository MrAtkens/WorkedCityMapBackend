using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
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
        public AdministrationAuthController(AdministrationAuthService administrationAuthService, ModeratorsCrudService moderatorsCrudService, AdminsCrudService adminCrudService, ILogger<AdministrationAuthController> logger)
        {
            this.logger = logger;
            this.administrationAuthService = administrationAuthService;
            this.adminCrudService = adminCrudService;
            this.moderatorsCrudService = moderatorsCrudService;

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminAuthenticate([FromBody]AdministartionAuthDTO adminAuthDTO)
        {
            try
            {
                
                
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Admin existingAdmin = await adminCrudService.CheckAdminExist(adminAuthDTO.Login);

                if (existingAdmin == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }

                Admin admin = administrationAuthService.AdminAuthenticate(existingAdmin, adminAuthDTO.Password);

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
        [AllowAnonymous]
        public async Task<IActionResult> ModeratorAuthenticate([FromBody]AdministartionAuthDTO adminAuthDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Moderator existingModerator = await moderatorsCrudService.CheckModeratorExist(adminAuthDTO.Login);

                if (existingModerator == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }

                Moderator moderator = administrationAuthService.ModeratorsAuthenticate(existingModerator, adminAuthDTO.Password);

                if (moderator == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }

                return Ok(new { moderator });
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
