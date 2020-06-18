using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Models.AuthModels;
using AuthJWT.Options;
using AuthJWT.Services.PublicPins;
using DTOs.DTOs;
using DTOs.DTOs.Pins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Services.AdministartionAccountsService;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [Authorize(Roles = Role.Moderator)]
    [ApiController]
    public class ModerationController : ControllerBase
    {
        private readonly ILogger<ModerationController> logger;
        private readonly ModerationPinService moderationPinService;
        private readonly ModeratorsCrudService moderatorsCrudService;
        public ModerationController(ILogger<ModerationController> logger, ModerationPinService moderationPinService, ModeratorsCrudService moderatorsCrudService)
        {
            this.logger = logger;
            this.moderatorsCrudService = moderatorsCrudService;
            this.moderationPinService = moderationPinService;

            logger.LogDebug(1, "NLog injected into ModerationController");
        }

        [HttpGet]
        public async Task<IActionResult> GetModerationPins()
        {
            
            try
            {
                List<ProblemPin> moderateProblemPins = await moderationPinService.GetModerationPins();
                ResponseDTO answer = new ResponseDTO() { Status = true };
                return Ok(new { moderateProblemPins, answer });
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModerationPinById(Guid id)
        {
            try
            {
                ProblemPin moderateProblemPin = await moderationPinService.GetModerationPinById(id);
                if (moderateProblemPin == null)
                {
                    logger.LogInformation($"GetModerationPinById dont found Id: {id}");
                    return NotFound(new ResponseDTO() { Message = "Пин не найден", Status = false });
                }
                ResponseDTO answer = new ResponseDTO() { Status = true };
                return Ok(new { moderateProblemPin, answer });
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

        [HttpPost]
        public async Task<IActionResult> AcceptPublicPin([FromBody]AcceptDTO acceptDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Moderator moderator = await moderatorsCrudService.GetModeratorById(acceptDTO.ModeratorId);

                ResponseDTO answer = await moderationPinService.ModerationAcceptPin(acceptDTO, moderator);
                logger.LogInformation($"Модератор: {acceptDTO.ModeratorId}, проверил и подтвердил пин: {acceptDTO.Id}");
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
                return StatusCode(500, new ResponseDTO() { Message = "На данный момент на стороне сервера ошибка сообщите администратору",
                Status = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptSolvedPin([FromBody]SolvedPinDTO solvedPinDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                ResponseDTO answer = await moderationPinService.SolvedProblemPinAccept(solvedPinDTO);
                logger.LogInformation($"Модератор: {solvedPinDTO.ModeratorLogin}: {solvedPinDTO.ModeratorId}, проверил работу команды *** над пином: {solvedPinDTO.Id}");
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


        [HttpPatch("{id}")]
        public async Task<IActionResult> EditModerationPin(Guid Id, [FromBody] EditProblemPinDTO moderateProblemPinDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                ProblemPin moderateProblemPin = await moderationPinService.GetModerationPinById(Id);
                if (moderateProblemPin == null)
                {
                    logger.LogInformation($"GetModerationPinById dont found Id: {Id}");
                    return NotFound(new ResponseDTO() { Message = "Пин не найден", Status = false });
                }

                ResponseDTO answer = await moderationPinService.EditModerationPin(moderateProblemPin, moderateProblemPinDTO);
                logger.LogInformation($"Модератор: {moderateProblemPinDTO.ModeratorLogin} : {moderateProblemPinDTO.ModeratorId}, проверил и подтвердил пин: {moderateProblemPin.Id}");

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
        public async Task<IActionResult> DeleteModerationPin(Guid Id)
        {
            try
            {
                ProblemPin moderateProblemPin = await moderationPinService.GetModerationPinById(Id);
                if (moderateProblemPin == null)
                {
                    logger.LogInformation($"GetModerationPinById dont found Id: {Id}");
                    return NotFound(new ResponseDTO() { Message = "Пин не найден", Status = false });
                }
                ResponseDTO answer = await moderationPinService.DeleteModerationPin(moderateProblemPin);
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