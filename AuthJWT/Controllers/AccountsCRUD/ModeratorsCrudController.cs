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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthJWT.Controllers.ModeratersCRUD
{
    [Route("api/[controller]/[action]")]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [Authorize(Roles = Role.Admin)]
    [ApiController]
    public class ModeratorsCrudController : ControllerBase
    {
        private readonly ILogger<ModeratorsCrudController> logger;
        private readonly ModeratorsCrudService moderatorsCrudService;
        private readonly AdminsCrudService adminsCrudService;
        public ModeratorsCrudController(ModeratorsCrudService moderatorsCrudService, AdminsCrudService adminsCrudService, ILogger<ModeratorsCrudController> logger)
        {
            this.logger = logger;
            this.moderatorsCrudService = moderatorsCrudService;
            this.adminsCrudService = adminsCrudService;
        }

        [HttpGet]
        public async Task<IActionResult> GetModerators()
        {
            try
            {
                List<Moderator> moderators = await moderatorsCrudService.GetModerators();
                return Ok(new { moderators });
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
        public async Task<IActionResult> GetModeratorById(Guid id)
        {
            try
            {
                Moderator moderator = await moderatorsCrudService.GetModeratorById(id);
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

        [HttpPost]
        public async Task<IActionResult> AddModerator([FromBody]ModeratorDTO addModeratorDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                ResponseDTO check = await moderatorsCrudService.CheckModeratorExistForAdd(addModeratorDTO.Login);

                if (check.Status == false)
                {
                    return StatusCode(400, check);
                }

                check = await adminsCrudService.CheckAdminExistForAdd(addModeratorDTO.Login);

                if (check.Status == false)
                {
                    return StatusCode(400, check);
                }

                Admin admin = await adminsCrudService.CheckAdminExist(addModeratorDTO.AdminId);

                ResponseDTO answer = await moderatorsCrudService.AddModerator(admin, addModeratorDTO);
                logger.LogInformation($"Модератор: {addModeratorDTO.Login}, был добавлен Админом {admin.Login}");

                return Ok(new { answer });
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
        public async Task<IActionResult> EditModerator(Guid Id, [FromForm] EditModeratorDTO moderatorEditDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Moderator foundedModerator = await moderatorsCrudService.CheckModeratorExist(Id);

                if (foundedModerator == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }

                ResponseDTO check = await moderatorsCrudService.EditModerator(foundedModerator, moderatorEditDTO);
                logger.LogInformation($"Модератор: {foundedModerator.Login}, был изменён Админом {moderatorEditDTO.AdminLogin}: {moderatorEditDTO.AdminId}");
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
        public async Task<IActionResult> DeleteModerator(Guid Id, AdminInfoDTO adminInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Moderator foundedModerator = await moderatorsCrudService.CheckModeratorExist(Id);

                if (foundedModerator == null)
                {
                    return NotFound(new ResponseDTO() { Message = "Данный пользователь не найден", Status = false });
                }

                ResponseDTO check = await moderatorsCrudService.DeleteModerator(foundedModerator);
                logger.LogInformation($"Данные модератора: {foundedModerator.Login}, были удалены {adminInfo.Login}: {adminInfo.Id}");
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
