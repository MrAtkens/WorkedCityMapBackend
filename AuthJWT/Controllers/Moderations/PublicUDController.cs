using System;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using AuthJWT.Models;
using AuthJWT.Options;
using AuthJWT.Services;
using DTOs.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [Authorize(Roles = Role.Moderator)]
    [ApiController]
    public class PublicUDController : ControllerBase
    {
        private readonly ILogger<PublicUDController> logger;
        private readonly PublicPinServiceCRUD publicPinServiceCRUD;
        public PublicUDController(ILogger<PublicUDController> logger, PublicPinServiceCRUD publicPinServiceCRUD)
        {
            this.logger = logger;
            this.publicPinServiceCRUD = publicPinServiceCRUD;

            logger.LogDebug(1, "NLog injected into PublicUDController");
        }

        [HttpPatch("{id}")]
       public async Task<IActionResult> EditPublicPin(Guid Id, [FromBody] ProblemPin problemPin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                ResponseDTO answer = await publicPinServiceCRUD.EditPublicPin(Id, problemPin);
                return Ok(answer);
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Пин не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка сообщите администратору",
                    Status = false
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublicPin(Guid Id)
        {
            try
            {
                ResponseDTO answer = await publicPinServiceCRUD.DeletePublicPin(Id);
                return Ok(answer);
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Пин не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new ResponseDTO()
                {
                    Message = "На данный момент на стороне сервера ошибка сообщите администратору",
                    Status = false
                });
            }
         }
        

    }
}