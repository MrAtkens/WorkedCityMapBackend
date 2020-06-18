using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using AuthJWT.DTOs;
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
    [EnableCors(CorsOrigins.FrontPolicy)]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly ILogger<PublicController> logger;
        private readonly PublicPinServiceGet publicPinServiceGet;
        private readonly PublicPinServiceCRUD publicPinServiceCRUD;
        public PublicController(ILogger<PublicController> logger, PublicPinServiceGet publicPinServiceGet, PublicPinServiceCRUD publicPinServiceCRUD)
        {
            this.logger = logger;
            this.publicPinServiceGet = publicPinServiceGet;
            this.publicPinServiceCRUD = publicPinServiceCRUD;

            logger.LogDebug(1, "NLog injected into PublicController");
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicMapPins()
        {
            try
            {
                List<ProblemPin> publicPins = await publicPinServiceGet.GetPublicPins();
                ResponseDTO answer = new ResponseDTO() { Status = true };
                return Ok(new { publicPins, answer });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new { status = false });
            }
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetPublicMapPinById(Guid id)
        {
            try
            {
                ProblemPin problemPin = await publicPinServiceCRUD.GetPublicPinById(id);
                if (problemPin == null)
                {
                    logger.LogInformation($"GetPublicMapPinById dont found Id: {id}");
                    return NotFound(new ResponseDTO() { Message = "Пин не найден", Status = false });
                }
                ResponseDTO answer = new ResponseDTO() { Status = true };
                return Ok(new { problemPin, answer });
            }
            catch (ObjectNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(404, new ResponseDTO() { Message = "Пин не найден", Status = false });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, false);
            }
         }

        [HttpPost]
        [Authorize(Roles = Role.User)]
        public async Task<IActionResult> CreateProblemPin([FromForm]ProblemPinDTO problemPinDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                ResponseDTO answer = await publicPinServiceCRUD.AddPublicPin(problemPinDTO, problemPinDTO.UserId);
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
                return StatusCode(500, false);
            }
        }


    }
}