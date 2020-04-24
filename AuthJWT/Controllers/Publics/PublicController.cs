using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthJWT.DTOs;
using AuthJWT.Models;
using AuthJWT.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace AuthJWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("FrontPolicy")]
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
                return Ok(new { publicPins, status = true });
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
                    return NotFound(new { status = false });
                }
                return Ok(new { problemPin, status = true });
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, false);
            }
         }

        [HttpPost]
        public async Task<IActionResult> CreateProblemPin([FromForm]ProblemPinDTO problemPinDTO)
        {
            try
            {

                string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                bool answer = await publicPinServiceCRUD.AddPublicPin(problemPinDTO, ip);
                return Ok(new { answer });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, false);
            }
        }


    }
}